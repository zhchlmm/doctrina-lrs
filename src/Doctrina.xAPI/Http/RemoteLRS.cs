using Doctrina.xAPI.Documents;
using Doctrina.xAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public XAPIVersion Version { get; }

        public RemoteLRS(string endpoint, string username, string password)
            : this(endpoint, username, password, XAPIVersion.Latest())
        {
        }

        public RemoteLRS(string endpoint, string username, string password, XAPIVersion version)
        {
            BaseAddress = new Uri(endpoint.TrimEnd('/'));

            var bytes = Encoding.UTF8.GetBytes($"{username}:{password}");
            _auth = Convert.ToBase64String(bytes);

            client.BaseAddress = BaseAddress;
            client.DefaultRequestHeaders.Add(Constants.Headers.XExperienceApiVersion, version.ToString());
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
            var result = new StatementsResult();
            var parameters = query.ToParameterMap(Version);

            var uriBuilder = new UriBuilder(BaseAddress);
            uriBuilder.Path += "/statements";
            uriBuilder.Query = parameters.ToString();
            var response = await client.GetAsync(uriBuilder.Uri);

            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);

            var contentType = response.Content.Headers.ContentType;
            var mediaType = contentType.MediaType;

            if (mediaType == MediaTypes.Application.Json)
            {
                string str = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<StatementsResult>(str);
            }
            else if(mediaType == MediaTypes.Multipart.Mixed)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotSupportedException($"Response content-type: '{contentType}' is not supported.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public async Task<StatementsResult> MoreStatements(StatementsResult result)
        {
            if (result.More == null)
                return null;

            var requestUri = new Uri(BaseAddress, result.More);
            var response = await client.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);

            var contentType = response.Content.Headers.ContentType;
            var mediaType = contentType.MediaType;

            if (mediaType == MediaTypes.Application.Json)
            {
                string str = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<StatementsResult>(str);
            }
            else if (mediaType == MediaTypes.Multipart.Mixed)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotSupportedException($"Response content-type: '{contentType}' is not supported.");
            }
        }

        public async Task<Statement> SaveStatement(Statement statement)
        {
            var uriBuilder = new UriBuilder(BaseAddress);
            uriBuilder.Path += "/statements";
            var stringContent = new StringContent(statement.ToJson(), Encoding.UTF8, MediaTypes.Application.Json);

            statement.Stamp();

            var response = await client.PostAsync(uriBuilder.Uri, stringContent);

            return statement;
        }

        public async Task<Statement[]> SaveStatements(Statement[] statements)
        {
            var uriBuilder = new UriBuilder(BaseAddress);
            uriBuilder.Path += "/statements";
            var serializedObject = JsonConvert.SerializeObject(statements);
            var stringContent = new StringContent(serializedObject, Encoding.UTF8, MediaTypes.Application.Json);

            var response = await client.PostAsync(uriBuilder.Uri, stringContent);

            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);

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
        public async Task<Statement> GetStatement(Guid id)
        {
            var uriBuilder = new UriBuilder(BaseAddress);
            uriBuilder.Path += "/statements";
            uriBuilder.Query = $"statementId={id}";

            var response = await client.GetAsync(uriBuilder.Uri);

            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);

            string strResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Statement>(strResponse);
        }

        /// <summary>
        /// Gets a voided statement
        /// </summary>
        /// <param name="id">Id of the voided statement</param>
        /// <returns>A voided statement</returns>
        public async Task<Statement> GetVoidedStatement(Guid id)
        {
            var uriBuilder = new UriBuilder(BaseAddress);
            uriBuilder.Path += "/statements";
            uriBuilder.Query += $"voidedStatementId={id}";

            var response = await client.GetAsync(uriBuilder.Uri);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);

            string strResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Statement>(strResponse);
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
                query.Add("registration", registration.Value.ToString(Constants.Formats.DateTimeFormat));

            builder.Query = query.ToString();

            var response = await client.GetAsync(builder.Uri);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);

            string strResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Guid[]>(strResponse);
        }

        public async Task<StateDocument> GetState(string stateId, Iri activityId, Agent agent, Guid? registration = null)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/activities/state";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("stateId", stateId);
            query.Add("activityId", activityId.ToString());
            query.Add("agent", agent.ToString());

            if (registration.HasValue)
                query.Add("registration", registration.Value.ToString(Constants.Formats.DateTimeFormat));

            builder.Query = query.ToString();

            var response = await client.GetAsync(builder.Uri);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);

            var state = new StateDocument();
            state.Content = await response.Content.ReadAsByteArrayAsync();
            state.ContentType = response.Content.Headers.ContentType;
            state.Activity = new Activity() { Id = activityId };
            state.Agent = agent;
            state.ETag = response.Headers.ETag;
            state.LastModified = response.Content.Headers.LastModified;
            return state;
        }

        public async Task SaveState(StateDocument state, ETagMatch? matchType = null)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/activities/state";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("stateId", state.Id);
            query.Add("activityId", state.Activity.Id.ToString());
            query.Add("agent", state.Agent.ToString());

            if (state.Registration.HasValue)
                query.Add("registration", state.Registration.Value.ToString(Constants.Formats.DateTimeFormat));

            builder.Query = query.ToString();

            var request = new HttpRequestMessage(HttpMethod.Delete, builder.Uri);

            // TOOD: Concurrency
            if (matchType.HasValue)
            {
                if (state.ETag == null)
                    throw new NullReferenceException("ETag");

                switch (matchType.Value)
                {
                    case ETagMatch.IfMatch:
                        request.Headers.IfMatch.Add(state.ETag);
                        break;
                    case ETagMatch.IfNoneMatch:
                        request.Headers.IfNoneMatch.Add(state.ETag);
                        break;
                }
            }

            request.Content = new ByteArrayContent(state.Content);
            request.Content.Headers.ContentType = state.ContentType;

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);
        }

        public async Task DeleteState(StateDocument state, ETagMatch? matchType = null)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/activities/state";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("stateId", state.Id);
            query.Add("activityId", state.Activity.Id.ToString());
            query.Add("agent", state.Agent.ToString());

            if (state.Registration.HasValue)
                query.Add("registration", state.Registration.Value.ToString(Constants.Formats.DateTimeFormat));

            builder.Query = query.ToString();

            var request = new HttpRequestMessage(HttpMethod.Delete, builder.Uri);

            // TOOD: Concurrency
            if (matchType.HasValue)
            {
                if (state.ETag == null)
                    throw new NullReferenceException("ETag");

                switch (matchType.Value)
                {
                    case ETagMatch.IfMatch:
                        request.Headers.IfMatch.Add(state.ETag);
                        break;
                    case ETagMatch.IfNoneMatch:
                        request.Headers.IfNoneMatch.Add(state.ETag);
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
                query.Add("registration", registration.Value.ToString(Constants.Formats.DateTimeFormat));

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
                query.Add("since", since.Value.ToString(Constants.Formats.DateTimeFormat));
            builder.Query = query.ToString();
            
            var response = await client.GetAsync(builder.Uri);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);

            string strResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Guid[]>(strResponse);
        }

        public async Task<ActivityProfileDocument> GetActivityProfile(string id, Iri activityId)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/activities/profile";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("profileId", id);
            query.Add("activityId", activityId.ToString());

            builder.Query = query.ToString();

            var response = await client.GetAsync(builder.Uri);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);

            var profile = new ActivityProfileDocument();
            profile.Id = id;
            profile.ActivityId = activityId;
            profile.Content = await response.Content.ReadAsByteArrayAsync();
            profile.ContentType = response.Content.Headers.ContentType;
            profile.ETag = response.Headers.ETag;
            profile.LastModified = response.Content.Headers.LastModified;
            return profile;
        }

        public async Task SaveActivityProfile(ActivityProfileDocument profile, ETagMatch? matchType = null)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/activities/profile";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("profileId", profile.Id);
            query.Add("activityId", profile.ActivityId.ToString());

            if (profile.Registration.HasValue)
                query.Add("registration", profile.Registration.Value.ToString(Constants.Formats.DateTimeFormat));

            builder.Query = query.ToString();

            var request = new HttpRequestMessage(HttpMethod.Post, builder.Uri);

            if (matchType.HasValue)
            {
                if (profile.ETag == null)
                    throw new NullReferenceException("ETag");

                switch (matchType.Value)
                {
                    case ETagMatch.IfMatch:
                        request.Headers.IfMatch.Add(profile.ETag);
                        break;
                    case ETagMatch.IfNoneMatch:
                        request.Headers.IfNoneMatch.Add(profile.ETag);
                        break;
                }
            }

            request.Content = new ByteArrayContent(profile.Content);
            request.Content.Headers.ContentType = profile.ContentType;

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);
        }

        public async Task DeleteActivityProfile(ActivityProfileDocument profile, ETagMatch? matchType = null)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/activities/profile";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("profileId", profile.Id);
            query.Add("activityId", profile.ActivityId.ToString());

            builder.Query = query.ToString();

            var request = new HttpRequestMessage(HttpMethod.Delete, builder.Uri);

            if (matchType.HasValue)
            {
                if (profile.ETag == null)
                    throw new NullReferenceException("ETag");

                switch (matchType.Value)
                {
                    case ETagMatch.IfMatch:
                        request.Headers.IfMatch.Add(profile.ETag);
                        break;
                    case ETagMatch.IfNoneMatch:
                        request.Headers.IfNoneMatch.Add(profile.ETag);
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

        public async Task<AgentProfileDocument> GetAgentProfile(string id, Agent agent)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/agents/profile";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("profileId", id);
            query.Add("agent", agent.ToString());

            builder.Query = query.ToString();

            var response = await client.GetAsync(builder.Uri);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);

            var profile = new AgentProfileDocument();
            profile.Id = id;
            profile.Agent = agent;
            profile.Content = await response.Content.ReadAsByteArrayAsync();
            profile.ContentType = response.Content.Headers.ContentType;
            profile.ETag = response.Headers.ETag;
            profile.LastModified = response.Content.Headers.LastModified;
            return profile;
        }

        public async Task SaveAgentProfile(AgentProfileDocument profile, ETagMatch? matchType = null)
        {
            var builder = new UriBuilder(BaseAddress);
            builder.Path += "/agents/profile";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("profileId", profile.Id);
            query.Add("agent", profile.Agent.ToString());

            builder.Query = query.ToString();

            var request = new HttpRequestMessage(HttpMethod.Post, builder.Uri);

            if (matchType.HasValue)
            {
                if (profile.ETag == null)
                    throw new NullReferenceException("ETag");

                switch (matchType.Value)
                {
                    case ETagMatch.IfMatch:
                        request.Headers.IfMatch.Add(profile.ETag);
                        break;
                    case ETagMatch.IfNoneMatch:
                        request.Headers.IfNoneMatch.Add(profile.ETag);
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
            query.Add("profileId", profile.Id);
            query.Add("agent", profile.Agent.ToString());

            builder.Query = query.ToString();

            var request = new HttpRequestMessage(HttpMethod.Delete, builder.Uri);

            if (matchType.HasValue)
            {
                if (profile.ETag == null)
                    throw new NullReferenceException("ETag");

                switch (matchType.Value)
                {
                    case ETagMatch.IfMatch:
                        request.Headers.IfMatch.Add(profile.ETag);
                        break;
                    case ETagMatch.IfNoneMatch:
                        request.Headers.IfNoneMatch.Add(profile.ETag);
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
