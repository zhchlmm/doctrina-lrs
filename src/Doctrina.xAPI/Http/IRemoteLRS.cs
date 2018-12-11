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
        Task<About> GetAbout();

        Task<Statement> SaveStatement(Statement statement);
        Task<Statement[]> SaveStatements(IEnumerable<Statement> statement);
        Task<Statement> GetStatement(Guid id);
        Task<Statement> GetVoidedStatement(Guid id);
        Task<StatementsResult> QueryStatements(StatementsQuery query);
        Task<StatementsResult> MoreStatements(StatementsResult result);
        Task<Statement> VoidStatement(Guid id, Agent agent);

        Task<Guid[]> GetStateIds(Activity activity, Agent agent, Guid? registration = null);
        Task<StateDocument> GetState(string id, Activity activity, Agent agent, Guid? registration = null);
        Task SaveState(StateDocument state);
        Task DeleteState(StateDocument state);
        Task ClearState(Activity activity, Agent agent, Guid? registration = null);

        Task<Guid[]> GetActivityProfileIds(Activity activity);
        Task<ActivityProfileDocument> GetActivityProfile(string id, Activity activity);
        Task SaveActivityProfile(ActivityProfileDocument profile);
        Task DeleteActivityProfile(ActivityProfileDocument profile);

        Task<Guid[]> GetAgentProfileIds(Agent agent);
        Task<AgentProfileDocument> GetAgentProfile(string id, Agent agent);
        Task SaveAgentProfile(AgentProfileDocument profile);
        Task DeleteAgentProfile(AgentProfileDocument profile);
    }
}