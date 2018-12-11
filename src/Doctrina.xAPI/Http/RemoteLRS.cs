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

        public Uri Endpoint { get; }
        public XAPIVersion Version { get; }

        public RemoteLRS(string endpoint, string username, string password)
            : this(endpoint, username, password, XAPIVersion.Latest())
        {
        }

        public RemoteLRS(string endpoint, string username, string password, XAPIVersion version)
        {
            Endpoint = new Uri(endpoint.TrimEnd('/'));

            var bytes = Encoding.UTF8.GetBytes($"{username}:{password}");
            _auth = Convert.ToBase64String(bytes);

            client.BaseAddress = Endpoint;
            client.DefaultRequestHeaders.Add(Constants.Headers.XExperienceApiVersion, version.ToString());
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _auth);
        }

        public async Task<About> GetAbout()
        {
            var relativeUri = new Uri("/about");
            var requestUri = new Uri(Endpoint, relativeUri);
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
            var relativeUri = new Uri("/statements?" + parameters.ToString());
            var requestUri = new Uri(Endpoint, relativeUri);
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

            var requestUri = new Uri(Endpoint, result.More);
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

        public async Task SaveStatement(Statement statement)
        {
            var requestUri = new Uri(Endpoint, "/statements");
            var stringContent = new StringContent(statement.ToJson(), Encoding.UTF8, MediaTypes.Application.Json);

            statement.Stamp();

            var response = await client.PostAsync(requestUri, stringContent);
        }

        public async Task SaveStatements(Statement[] statements)
        {
            var requestUri = new Uri(Endpoint, "/statements");
            var serializedObject = JsonConvert.SerializeObject(statements);
            var stringContent = new StringContent(serializedObject, Encoding.UTF8, MediaTypes.Application.Json);

            var response = await client.PostAsync(requestUri, stringContent);

            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);

            string strResponse = await response.Content.ReadAsStringAsync();

            var ids = JsonConvert.DeserializeObject<Guid[]>(strResponse);

            for (int i = 0; i < statements.Count(); i++)
            {
                statements[i].Id = ids[i];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Statement> GetStatement(Guid id)
        {
            var requestUri = new Uri(Endpoint, $"/statements?statementId={id}");
            var response = await client.GetAsync(requestUri);

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
            var requestUri = new Uri(Endpoint, $"/statements?voidedStatementId={id}");
            var response = await client.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);

            string strResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Statement>(strResponse);
        }

        public async Task VoidStatement(Guid id, Agent agent)
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

            await SaveStatement(voidStatement);
        }
        #endregion

        #region Activity State
        public async Task<Guid[]> GetStateIds(Iri activityId, Agent agent, Guid? registration = null)
        {
            var builder = new UriBuilder(Endpoint);
            builder.Path = "/activities/profile";

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
            var builder = new UriBuilder(Endpoint);
            builder.Path = "/activities/profile";

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

        public async Task SaveState(StateDocument state)
        {
            var builder = new UriBuilder(Endpoint);
            builder.Path = "/activities/profile";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("stateId", state.Id);
            query.Add("activityId", state.Activity.Id.ToString());
            query.Add("agent", state.Agent.ToString());

            if (state.Registration.HasValue)
                query.Add("registration", state.Registration.Value.ToString(Constants.Formats.DateTimeFormat));

            builder.Query = query.ToString();

            var content = new ByteArrayContent(state.Content);
            content.Headers.ContentType = state.ContentType;
            var response = await client.PostAsync(builder.Uri, content);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);
        }

        public async Task DeleteState(StateDocument state)
        {
            var builder = new UriBuilder(Endpoint);
            builder.Path = "/activities/profile";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("stateId", state.Id);
            query.Add("activityId", state.Activity.Id.ToString());
            query.Add("agent", state.Agent.ToString());

            if (state.Registration.HasValue)
                query.Add("registration", state.Registration.Value.ToString(Constants.Formats.DateTimeFormat));

            builder.Query = query.ToString();

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Delete,
                RequestUri = builder.Uri,
            };

            if (state.ETag != null)
                request.Headers.IfMatch.Add(state.ETag);
            
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);
        }

        public Task ClearState(Activity activity, Agent agent, Guid? registration = null)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ActivityProfile
        public async Task<Guid[]> GetActivityProfileIds(Iri activityId, DateTimeOffset? since = null)
        {

            var builder = new UriBuilder(Endpoint);
            builder.Path = "/activities/profile";

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

        public Task<ActivityProfileDocument> GetActivityProfile(string id, Activity activity)
        {
            throw new NotImplementedException();
        }

        public Task SaveActivityProfile(ActivityProfileDocument profile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteActivityProfile(ActivityProfileDocument profile)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region AgentProfile
        public Task<Guid[]> GetAgentProfileIds(Agent agent)
        {
            throw new NotImplementedException();
        }

        public Task<AgentProfileDocument> GetAgentProfile(string id, Agent agent)
        {
            throw new NotImplementedException();
        }

        public Task SaveAgentProfile(AgentProfileDocument profile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAgentProfile(AgentProfileDocument profile)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    public class AttachmentsCollection : Dictionary<string, AttachmentBatch>
    {

    }
}
