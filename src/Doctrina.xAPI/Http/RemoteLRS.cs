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

        public Task<HttpResponseMessage> GetAbout()
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> QueryStatements(StatementsQuery query)
        {
            var result = new StatementsResult();
            var parameters = query.ToParameterMap(Version);
            var relativeUri = new Uri("/statements?" + parameters.ToString());
            var requestUri = new Uri(Endpoint, relativeUri);
            var response = await client.GetAsync(requestUri);
            return response;

            //IEnumerable<string> contentTypeHeaderValue = null;
            //if (!response.Headers.TryGetValues("Content-Type", out contentTypeHeaderValue))
            //{
            //    throw new NullReferenceException("Response missing Content-Type header.");
            //}

            //string contentType = contentTypeHeaderValue.First();
            //if(contentType == MIMETypes.Application.Json)
            //{
            //    string stringContent = await response.Content.ReadAsStringAsync();
            //    return JsonConvert.DeserializeObject<StatementsResult>(stringContent);
            //}else if(contentType == MIMETypes.Multipart.Mixed)
            //{
            //    var multipartContent = (MultipartContent)response.Content;
            //    for (int i = 0; i < multipartContent.Count(); i++)
            //    {
            //        if(i == 0)
            //        {

            //        }

            //        var subContent = multipartContent.ElementAt(i);
            //        string subContentType = subContent.Headers.ContentType.MediaType;
            //        IEnumerable<string> hashValues = null;
            //        if (!subContent.Headers.TryGetValues(Constants.Headers.XExperienceAPIHash, out hashValues))
            //        {
            //            throw new Exception($"Subtype at [{i}] is missing header '{Constants.Headers.XExperienceAPIHash}'.");
            //        }
            //        string hash = hashValues.First();
            //    }
                
            //}

            //throw new InvalidOperationException($"Response content-type: '{contentType}' is not supported.");
        }

        public Task<HttpResponseMessage> SaveStatement(Statement statement)
        {
            var requestUri = new Uri(Endpoint, "/statemnts");
            var stringContent = new StringContent(statement.ToJson(), Encoding.UTF8, MediaTypes.Application.Json);
            return client.PostAsync(requestUri, stringContent);
        }

        public Task<HttpResponseMessage> SaveStatements(IEnumerable<Statement> statements)
        {
            var requestUri = new Uri(Endpoint, "/statemnts");
            var serializedObject = JsonConvert.SerializeObject(statements);
            var stringContent = new StringContent(serializedObject, Encoding.UTF8, MediaTypes.Application.Json);
            return client.PostAsync(requestUri, stringContent);
        }

        public Task<HttpResponseMessage> GetStatement(Guid id)
        {
            var requestUri = new Uri(Endpoint, $"/statemnts?statementId={id}");
            var response = client.GetAsync(requestUri);
            return response;
        }

        public Task<HttpResponseMessage> GetVoidedStatement(Guid id)
        {
            var requestUri = new Uri(Endpoint, $"/statemnts?voidedStatementId={id}");
            var response = client.GetAsync(requestUri);
            return response;
        }

        public Task<HttpResponseMessage> MoreStatements(StatementsResult result)
        {
            //if (result.More.IsAbsoluteUri)
            //{
            //    var response = client.GetAsync(result.More);
            //    return response;
            //}
            //else
            //{
            var requestUri = new Uri(Endpoint, result.More);
            var response = client.GetAsync(requestUri);
            return response;
            //}
        }

        public Task<HttpResponseMessage> VoidStatement(Guid id, Agent agent)
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

            return SaveStatement(voidStatement);
        }

       
        #region Activity State
        public Task<HttpResponseMessage> GetStateIds(Activity activity, Agent agent, Guid? registration = null)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> GetState(string id, Activity activity, Agent agent, Guid? registration = null)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> SaveState(StateDocument state)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> DeleteState(StateDocument state)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> ClearState(Activity activity, Agent agent, Guid? registration = null)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ActivityProfile
        public Task<HttpResponseMessage> GetActivityProfileIds(Activity activity)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> GetActivityProfile(string id, Activity activity)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> SaveActivityProfile(ActivityProfileDocument profile)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> DeleteActivityProfile(ActivityProfileDocument profile)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region AgentProfile
        public Task<HttpResponseMessage> GetAgentProfileIds(Agent agent)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> GetAgentProfile(string id, Agent agent)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> SaveAgentProfile(AgentProfileDocument profile)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> DeleteAgentProfile(AgentProfileDocument profile)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    public class AttachmentsCollection : Dictionary<string, AttachmentBatch>
    {

    }
}
