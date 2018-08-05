using Doctrina.xAPI.Models;
using Doctrina.xAPI.Documents;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Http
{
    public interface IRemoteLRS
    {
        Task<HttpResponseMessage> SaveStatement(Statement statement);
        Task<HttpResponseMessage> SaveStatements(IEnumerable<Statement> statement);
        Task<HttpResponseMessage> GetStatement(Guid id);
        Task<HttpResponseMessage> GetVoidedStatement(Guid id);
        Task<HttpResponseMessage> QueryStatements(StatementsQuery query);
        Task<HttpResponseMessage> MoreStatements(StatementsResult result);
        Task<HttpResponseMessage> GetAbout();
        Task<HttpResponseMessage> VoidStatement(Guid id, Agent agent);

        Task<HttpResponseMessage> GetStateIds(Activity activity, Agent agent, Guid? registration = null);
        Task<HttpResponseMessage> GetState(String id, Activity activity, Agent agent, Guid? registration = null);
        Task<HttpResponseMessage> SaveState(StateDocument state);
        Task<HttpResponseMessage> DeleteState(StateDocument state);
        Task<HttpResponseMessage> ClearState(Activity activity, Agent agent, Nullable<Guid> registration = null);

        Task<HttpResponseMessage> GetActivityProfileIds(Activity activity);
        Task<HttpResponseMessage> GetActivityProfile(string id, Activity activity);
        Task<HttpResponseMessage> SaveActivityProfile(ActivityProfileDocument profile);
        Task<HttpResponseMessage> DeleteActivityProfile(ActivityProfileDocument profile);

        Task<HttpResponseMessage> GetAgentProfileIds(Agent agent);
        Task<HttpResponseMessage> GetAgentProfile(string id, Agent agent);
        Task<HttpResponseMessage> SaveAgentProfile(AgentProfileDocument profile);
        Task<HttpResponseMessage> DeleteAgentProfile(AgentProfileDocument profile);
    }
}