using Doctrina.Core.Data;

namespace Doctrina.Core.Repositories
{
    public interface IAgentRepository
    {
        //AgentEntity GetByAccount(string homePage, string name);
        //AgentEntity GetByMbox(string mbox);
        //AgentEntity GetByMboxSha1sum(string mboxSha1sum);
        //AgentEntity GetByOpenID(string openid);
        void Insert(AgentEntity entity);
        AgentEntity GetAgentOrGroup(AgentEntity actor);
    }
}