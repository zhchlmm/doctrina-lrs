using Doctrina.xAPI.Documents;
using System;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Http
{
    public interface IRemoteLRS
    {
        Task<About> GetAbout();

        Task<Statement> SaveStatement(Statement statement);
        Task PutStatement(Statement statement);
        Task<Statement[]> SaveStatements(Statement[] statement);
        Task<Statement> GetStatement(Guid id, bool attachments, ResultFormat format);
        Task<Statement> GetVoidedStatement(Guid id, bool attachments, ResultFormat format);
        Task<StatementsResult> QueryStatements(StatementsQuery query);
        Task<StatementsResult> MoreStatements(StatementsResult result);
        Task<Statement> VoidStatement(Guid id, Agent agent);

        Task<Guid[]> GetStateIds(Iri activityId, Agent agent, Guid? registration = null);
        Task<ActivityStateDocument> GetState(string id, Iri activityId, Agent agent, Guid? registration = null);
        Task SaveState(ActivityStateDocument state, ETagMatch? matchType = null);
        Task DeleteState(ActivityStateDocument state, ETagMatch? matchType = null);
        Task ClearState(Iri activityId, Agent agent, Guid? registration = null, ETagMatch? matchType = null);

        Task<Guid[]> GetActivityProfileIds(Iri activityId, DateTimeOffset? since = null);
        Task<ActivityProfileDocument> GetActivityProfile(string id, Iri activityId);
        Task SaveActivityProfile(ActivityProfileDocument profile, ETagMatch? matchType = null);
        Task DeleteActivityProfile(ActivityProfileDocument profile, ETagMatch? matchType = null);

        Task<Guid[]> GetAgentProfileIds(Agent agent);
        Task<AgentProfileDocument> GetAgentProfile(string id, Agent agent);
        Task SaveAgentProfile(AgentProfileDocument profile, ETagMatch? matchType = null);
        Task DeleteAgentProfile(AgentProfileDocument profile, ETagMatch? matchType = null);
    }
}