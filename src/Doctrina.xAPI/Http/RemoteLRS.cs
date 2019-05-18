using Doctrina.xAPI.Documents;
using Doctrina.xAPI.Json;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Doctrina.xAPI.Http
{
    public class RemoteLRS : IRemoteLRS
    {
        private readonly HttpClient client = new HttpClient();
        private readonly string _auth;

        public Uri BaseAddress { get; }
        public ApiVersion Version { get; }

        public RemoteLRS(string endpoint, string username, string password)
            : this(endpoint, username, password, ApiVersion.GetLatest())
        {
        }

        public RemoteLRS(string endpoint, string username, string password, ApiVersion version)
        {
            BaseAddress = new Uri(endpoint.TrimEnd('/'));

            var bytes = Encoding.UTF8.GetBytes($"{username}:{password}");
            _auth = Convert.ToBase64String(bytes);

            client.BaseAddress = BaseAddress;
            client.DefaultRequestHeaders.Add(Headers.XExperienceApiVersion, version.ToString());
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _auth);
        }

        public async Task<About> GetAbout()
        {
            var relativeUri = new Uri("/about");
            var requestUri = new Uri(BaseAddress, relativeUri);
            var response = await client.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);

            string str = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<About>(str);
        }

        #region Statements
        public async Task<StatementsResult> QueryStatements(StatementsQuery query)
        {
            if (query.StatementId.HasValue
                || query.VoidedStatementId.HasValue)
            {
                // Single statement response
                throw new ArgumentException("Use GetStatement or GetVoidedStatement methods indstead.");
            }

            var parameters = query.ToParameterMap(Version);

            var uriBuilder = new UriBuilder(BaseAddress);
            uriBuilder.Path += "/statements";
            uriBuilder.Query = parameters.ToString();
            var response = await client.GetAsync(uriBuilder.Uri);

            response.EnsureSuccessStatusCode();

            StatementsResultContent responseContent = response.Content as StatementsResultContent;

            return await responseContent.ReadAsStatementsResultAsync(ApiVersion.GetLatest());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public async Task<StatementsResult> MoreStatements(Iri more)
        {
            if (more == null)
                return null;

            var requestUri = new Uri(BaseAddress, (Uri)more);
            var response = await client.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();

            StatementsResultContent responseContent = response.Content as StatementsResultContent;

            return await responseContent.ReadAsStatementsResultAsync(ApiVersion.GetLatest());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public async Task<StatementsResult> MoreStatements(StatementsResult result)
        {
            return await MoreStatements((Iri)result.More);
        }

        public async Task<Statement> SaveStatement(Statement statement)
        {
            var uriBuilder = new UriBuilder(BaseAddress);
            uriBuilder.Path += "/statements";

            var jsonContent = new StringContent(statement.ToJson(), Encoding.UTF8, MediaTypes.Application.Json);

            HttpContent requestContent = jsonContent;

            var attachmentsWithPayload = statement.Attachments.Where(x => x.Payload != null);
            if (attachmentsWithPayload.Any())
            {
                var multipart = new MultipartContent("mixed");
                multipart.Add(jsonContent);

                foreach (var attachment in attachmentsWithPayload)
                {
                    multipart.Add(new AttachmentContent(attachment));
                }
            }
            else
            {
                requestContent = jsonContent;
            }

            var response = await client.PostAsync(uriBuilder.Uri, requestContent);

            return statement;
        }

        public async Task PutStatement(Statement statement)
        {
            var uriBuilder = new UriBuilder(BaseAddress);
            uriBuilder.Path += "/statements";
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query.Add("statementId", statement.Id.ToString());

            var jsonContent = new StringContent(statement.ToJson(), Encoding.UTF8, MediaTypes.Application.Json);

            HttpContent requestContent = jsonContent;

            var attachmentsWithPayload = statement.Attachments.Where(x => x.Payload != null);
            if (attachmentsWithPayload.Any())
            {
                var multipart = new MultipartContent("mixed");
                multipart.Add(jsonContent);

                foreach (var attachment in attachmentsWithPayload)
                {
                    multipart.Add(new AttachmentContent(attachment));
                }
            }
            else
            {
                requestContent = jsonContent;
            }

            var response = await client.PutAsync(uriBuilder.Uri, requestContent);

            response.EnsureSuccessStatusCode();
        }

        public async Task<Statement[]> SaveStatements(Statement[] statements)
        {
            var uriBuilder = new UriBuilder(BaseAddress);
            uriBuilder.Path += "/statements";

            var serializedObject = JsonConvert.SerializeObject(statements);
            var jsonContent = new StringContent(serializedObject, Encoding.UTF8, MediaTypes.Application.Json);

            HttpContent postContent = jsonContent;

            var attachmentsWithPayload = statements.SelectMany(s => s.Attachments.Where(x => x.Payload != null));
            if (attachmentsWithPayload.Any())
            {
                var multipartContent = new MultipartContent("mixed")
                {
                    jsonContent
                };

                foreach (var attachment in attachmentsWithPayload)
                {
                    multipartContent.Add(new AttachmentContent(attachment));
                }

                postContent = multipartContent;
            }

            var response = await client.PostAsync(uriBuilder.Uri, postContent);

            response.EnsureSuccessStatusCode();

            string strResponse = await response.Content.ReadAsStringAsync();

            var ids = JsonConvert.DeserializeObject<Guid[]>(strResponse);

            for (int i = 0; i < statements.Count(); i++)
            {
                statements[i].Id = ids[i];
            }

            return statements;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Statement> GetStatement(Guid id, bool attachments = false, ResultFormat format = ResultFormat.Exact)
        {
            var uriBuilder = new UriBuilder(BaseAddress);
            uriBuilder.Path += "/statements";
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query.Add("statementId", id.ToString());
            if (attachments == true)
                query.Add("attachments", "true");

            if (format != ResultFormat.Exact)
                query.Add("format", ResultFormat.Exact.ToString());

            uriBuilder.Query = query.ToString();

            var response = await client.GetAsync(uriBuilder.Uri);

            response.EnsureSuccessStatusCode();

            var contentType = response.Content.Headers.ContentType;
            if (contentType.MediaType == MediaTypes.Application.Json)
            {
                string strResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Statement>(strResponse);
            }
            else if (contentType.MediaType == MediaTypes.Multipart.Mixed)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var boundary = contentType.Parameters.SingleOrDefault(x => x.Name == "boundary");
                var multipart = new MultipartReader(boundary.Value, stream);
                var section = await multipart.ReadNextSectionAsync();
                int sectionIndex = 0;
                Statement statement = null;
                while (section != null)
                {
                    if (sectionIndex == 0)
                    {
                        string jsonString = await section.ReadAsStringAsync();
                        var serializer = new ApiJsonSerializer(ApiVersion.GetLatest());
                        var jsonReader = new JsonTextReader(new StringReader(jsonString));
                        statement = serializer.Deserialize<Statement>(jsonReader);
                    }
                    else
                    {
                        var attachmentSection = new MultipartAttachmentSection(section);
                        string hash = attachmentSection.XExperienceApiHash;
                        var attachment = statement.Attachments.FirstOrDefault(x => x.SHA2 == hash);
                    }

                    section = await multipart.ReadNextSectionAsync();
                    sectionIndex++;
                }

                return statement;
            }

            throw new Exception("Unsupported Content-Type response.");
        }

        /// <summary>
        /// Gets a voided statement
        /// </summary>
        /// <param name="id">Id of the voided statement</param>
        /// <returns>A voided statement</returns>
        public async Task<Statement> GetVoidedStatement(Guid id, bool attachments = false, ResultFormat format = ResultFormat.Exact)
        {
            var uriBuilder = new UriBuilder(BaseAddress);
            uriBuilder.Path += "/statements";
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query.Add("voidedStatementId", id.ToString());
            if (attachments == true)
                query.Add("attachments", "true");

            if (format != ResultFormat.Exact)
                query.Add("format", ResultFormat.Exact.ToString());

            var response = await client.GetAsync(uriBuilder.Uri);

            response.EnsureSuccessStatusCode();

            var contentType = response.Content.Headers.ContentType;
            if (contentType.MediaType == MediaTypes.Application.Json)
            {
                string strResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Statement>(strResponse);
            }
            else if (contentType.MediaType == MediaTypes.Multipart.Mixed)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var boundary = contentType.Parameters.SingleOrDefault(x => x.Name == "boundary");
                var multipart = new MultipartReader(boundary.Value, stream);
                var section = await multipart.ReadNextSectionAsync();
                int sectionIndex = 0;
                Statement statement = null;
                while (section != null)
                {
                    if (sectionIndex == 0)
                    {
                        string jsonString = await section.ReadAsStringAsync();
                        var serializer = new ApiJsonSerializer(ApiVersion.GetLatest());
                        var jsonReader = new JsonTextReader(new StringReader(jsonString));
                        statement = serializer.Deserialize<Statement>(jsonReader);
                    }
                    else
                    {
                        var attachmentSection = new MultipartAttachmentSection(section);
                        string hash = attachmentSection.XExperienceApiHash;
                        var attachment = statement.Attachments.FirstOrDefault(x => x.SHA2 == hash);
                    }

                    section = await multipart.ReadNextSectionAsync();
                    sectionIndex++;
                }

                return statement;
            }

            throw new Exception("Unsupported Content-Type response.");
        }

        /// <summary>
        /// Voids a statement
        /// </summary>
        /// <param name="id">Id of the statement to void.</param>
        /// <param name="agent">Agent who voids a statement.</param>
        /// <returns>Voiding statement</returns>
        public async Task<Statement> VoidStatement(Guid id, Agent agent)
        {
            var voidStatement = new Statement
            {
                Actor = agent,
                Verb = new Verb
                {
                    Id = new Iri("http://adlnet.gov/expapi/verbs/voided"),
                    Display = new LanguageMap()
                    {
                        { "en-US", "voided" }
                    }
                },
                Object = new StatementRef { Id = id }
            };

            return await SaveStatement(voidStatement);
        }
        #endregion

        #region Activity State
        public async Task<Guid[]> GetStateIds(Iri activityId, Agent agent, Guid? registration = null)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/activities/state";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("activityId", activityId.ToString());
            query.Add("agent", agent.ToString());

            if (registration.HasValue)
                query.Add("registration", registration.Value.ToString("o"));

            builder.Query = query.ToString();

            var response = await client.GetAsync(builder.Uri);

            response.EnsureSuccessStatusCode();

            string strResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Guid[]>(strResponse);
        }

        public async Task<ActivityStateDocument> GetState(string stateId, Iri activityId, Agent agent, Guid? registration = null)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/activities/state";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("stateId", stateId);
            query.Add("activityId", activityId.ToString());
            query.Add("agent", agent.ToString());

            if (registration.HasValue)
                query.Add("registration", registration.Value.ToString("o"));

            builder.Query = query.ToString();

            var response = await client.GetAsync(builder.Uri);

            response.EnsureSuccessStatusCode();

            var state = new ActivityStateDocument
            {
                Content = await response.Content.ReadAsByteArrayAsync(),
                ContentType = response.Content.Headers.ContentType.ToString(),
                Activity = new Activity() { Id = activityId },
                Agent = agent,
                Tag = response.Headers.ETag.Tag,
                LastModified = response.Content.Headers.LastModified
            };

            return state;
        }

        public async Task SaveState(ActivityStateDocument state, ETagMatch? matchType = null)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/activities/state";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("stateId", state.StateId);
            query.Add("activityId", state.Activity.Id.ToString());
            query.Add("agent", state.Agent.ToString());

            if (state.Registration.HasValue)
                query.Add("registration", state.Registration.Value.ToString("o"));

            builder.Query = query.ToString();

            var request = new HttpRequestMessage(HttpMethod.Delete, builder.Uri);

            // TOOD: Concurrency
            if (matchType.HasValue)
            {
                if (state.Tag == null)
                    throw new NullReferenceException("ETag");

                switch (matchType.Value)
                {
                    case ETagMatch.IfMatch:
                        request.Headers.IfMatch.Add(new EntityTagHeaderValue(state.Tag));
                        break;
                    case ETagMatch.IfNoneMatch:
                        request.Headers.IfNoneMatch.Add(new EntityTagHeaderValue(state.Tag));
                        break;
                }
            }

            request.Content = new ByteArrayContent(state.Content);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(state.ContentType);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);
        }

        public async Task DeleteState(ActivityStateDocument state, ETagMatch? matchType = null)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/activities/state";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("stateId", state.StateId);
            query.Add("activityId", state.Activity.Id.ToString());
            query.Add("agent", state.Agent.ToString());

            if (state.Registration.HasValue)
                query.Add("registration", state.Registration.Value.ToString("o"));

            builder.Query = query.ToString();

            var request = new HttpRequestMessage(HttpMethod.Delete, builder.Uri);

            // TOOD: Concurrency
            if (matchType.HasValue)
            {
                if (state.Tag == null)
                    throw new NullReferenceException("ETag");

                switch (matchType.Value)
                {
                    case ETagMatch.IfMatch:
                        request.Headers.IfMatch.Add(new EntityTagHeaderValue(state.Tag));
                        break;
                    case ETagMatch.IfNoneMatch:
                        request.Headers.IfNoneMatch.Add(new EntityTagHeaderValue(state.Tag));
                        break;
                }
            }

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);
        }

        public async Task ClearState(Iri activityId, Agent agent, Guid? registration = null, ETagMatch? matchType = null)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/activities/state";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("activityId", activityId.ToString());
            query.Add("agent", agent.ToString());

            if (registration.HasValue)
                query.Add("registration", registration.Value.ToString("o"));

            builder.Query = query.ToString();

            var request = new HttpRequestMessage(HttpMethod.Delete, builder.Uri);

            // TOOD: Concurrency
            //if (matchType.HasValue)
            //{
            //    if (profile.ETag == null)
            //        throw new NullReferenceException("ETag");

            //    switch (matchType.Value)
            //    {
            //        case ETagMatchType.IfMatch:
            //            request.Headers.IfMatch.Add(profile.ETag);
            //            break;
            //        case ETagMatchType.IfNoneMatch:
            //            request.Headers.IfNoneMatch.Add(profile.ETag);
            //            break;
            //    }
            //}

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);
        }
        #endregion

        #region ActivityProfile
        public async Task<Guid[]> GetActivityProfileIds(Iri activityId, DateTimeOffset? since = null)
        {

            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/activities/profile";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("activityId", activityId.ToString());
            if (since.HasValue)
                query.Add("since", since.Value.ToString("o"));
            builder.Query = query.ToString();

            var response = await client.GetAsync(builder.Uri);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);

            string strResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Guid[]>(strResponse);
        }

        public async Task<ActivityProfileDocument> GetActivityProfile(string profileId, Iri activityId)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/activities/profile";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("profileId", profileId);
            query.Add("activityId", activityId.ToString());

            builder.Query = query.ToString();

            var response = await client.GetAsync(builder.Uri);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);

            var profile = new ActivityProfileDocument();
            profile.ProfileId = profileId;
            profile.ActivityId = activityId;
            profile.Content = await response.Content.ReadAsByteArrayAsync();
            profile.ContentType = response.Content.Headers.ContentType.ToString();
            profile.Tag = response.Headers.ETag.ToString();
            profile.LastModified = response.Content.Headers.LastModified;
            return profile;
        }

        public async Task SaveActivityProfile(ActivityProfileDocument profile, ETagMatch? matchType = null)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/activities/profile";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("profileId", profile.ProfileId);
            query.Add("activityId", profile.ActivityId.ToString());

            if (profile.Registration.HasValue)
                query.Add("registration", profile.Registration.Value.ToString("o"));

            builder.Query = query.ToString();

            var request = new HttpRequestMessage(HttpMethod.Post, builder.Uri);

            if (matchType.HasValue)
            {
                if (profile.Tag == null)
                    throw new NullReferenceException("ETag");

                switch (matchType.Value)
                {
                    case ETagMatch.IfMatch:
                        request.Headers.IfMatch.Add(new EntityTagHeaderValue(profile.Tag));
                        break;
                    case ETagMatch.IfNoneMatch:
                        request.Headers.IfNoneMatch.Add(new EntityTagHeaderValue(profile.Tag));
                        break;
                }
            }

            request.Content = new ByteArrayContent(profile.Content);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(profile.ContentType);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);
        }

        public async Task DeleteActivityProfile(ActivityProfileDocument profile, ETagMatch? matchType = null)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/activities/profile";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("profileId", profile.ProfileId);
            query.Add("activityId", profile.ActivityId.ToString());

            builder.Query = query.ToString();

            var request = new HttpRequestMessage(HttpMethod.Delete, builder.Uri);

            if (matchType.HasValue)
            {
                if (profile.Tag == null)
                    throw new NullReferenceException("ETag");

                switch (matchType.Value)
                {
                    case ETagMatch.IfMatch:
                        request.Headers.IfMatch.Add(new EntityTagHeaderValue(profile.Tag));
                        break;
                    case ETagMatch.IfNoneMatch:
                        request.Headers.IfNoneMatch.Add(new EntityTagHeaderValue(profile.Tag));
                        break;
                }
            }

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);
        }
        #endregion

        #region Agent Profiles
        public async Task<Guid[]> GetAgentProfileIds(Agent agent)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/agents/profile";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("agent", agent.ToString());

            builder.Query = query.ToString();

            var response = await client.GetAsync(builder.Uri);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);

            string strResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Guid[]>(strResponse);
        }

        public async Task<AgentProfileDocument> GetAgentProfile(string profileId, Agent agent)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/agents/profile";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("profileId", profileId);
            query.Add("agent", agent.ToString());

            builder.Query = query.ToString();

            var response = await client.GetAsync(builder.Uri);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);

            var profile = new AgentProfileDocument();
            profile.ProfileId = profileId;
            profile.Agent = agent;
            profile.Content = await response.Content.ReadAsByteArrayAsync();
            profile.ContentType = response.Content.Headers.ContentType.ToString();
            profile.Tag = response.Headers.ETag.ToString();
            profile.LastModified = response.Content.Headers.LastModified;
            return profile;
        }

        public async Task SaveAgentProfile(AgentProfileDocument profile, ETagMatch? matchType = null)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/agents/profile";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("profileId", profile.ProfileId);
            query.Add("agent", profile.Agent.ToString());

            builder.Query = query.ToString();

            var request = new HttpRequestMessage(HttpMethod.Post, builder.Uri);

            if (matchType.HasValue)
            {
                if (profile.Tag == null)
                    throw new NullReferenceException("ETag");

                switch (matchType.Value)
                {
                    case ETagMatch.IfMatch:
                        request.Headers.IfMatch.Add(new EntityTagHeaderValue(profile.Tag));
                        break;
                    case ETagMatch.IfNoneMatch:
                        request.Headers.IfNoneMatch.Add(new EntityTagHeaderValue(profile.Tag));
                        break;
                }
            }

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);
        }

        public async Task DeleteAgentProfile(AgentProfileDocument profile, ETagMatch? matchType = null)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/agents/profile";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("profileId", profile.ProfileId);
            query.Add("agent", profile.Agent.ToString());

            builder.Query = query.ToString();

            var request = new HttpRequestMessage(HttpMethod.Delete, builder.Uri);

            if (matchType.HasValue)
            {
                if (profile.Tag == null)
                    throw new NullReferenceException("ETag");

                switch (matchType.Value)
                {
                    case ETagMatch.IfMatch:
                        request.Headers.IfMatch.Add(new EntityTagHeaderValue(profile.Tag));
                        break;
                    case ETagMatch.IfNoneMatch:
                        request.Headers.IfNoneMatch.Add(new EntityTagHeaderValue(profile.Tag));
                        break;
                }
            }

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);
        }
        #endregion
    }
}
