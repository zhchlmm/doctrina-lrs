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
        Task<Statement[]> SaveStatements(Statement[] statement);
        Task<Statement> GetStatement(Guid id);
        Task<Statement> GetVoidedStatement(Guid id);
        Task<StatementsResult> QueryStatements(StatementsQuery query);
        Task<StatementsResult> MoreStatements(StatementsResult result);
        Task<Statement> VoidStatement(Guid id, Agent agent);

        Task<Guid[]> GetStateIds(Iri activityId, Agent agent, Guid? registration = null);
        Task<StateDocument> GetState(string id, Iri activityId, Agent agent, Guid? registration = null);
        Task SaveState(StateDocument state, ETagMatch? matchType = null);
        Task DeleteState(StateDocument state, ETagMatch? matchType = null);
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