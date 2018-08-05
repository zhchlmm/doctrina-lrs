using Doctrina.Core;
using Doctrina.Core.Data;
using System.Linq;

namespace Doctrina.Core.Repositories
{
    public class AgentRepository : IAgentRepository
    {
        private readonly DoctrinaContext context;

        public AgentRepository(DoctrinaContext context) 
        {
            this.context = context;
        }

        public AgentEntity GetByMbox(string mbox)
        {
            return context.Agents.FirstOrDefault(x => x.Mbox == mbox);
        }

        public AgentEntity GetByMboxSha1sum(string mboxSha1sum)
        {
            return context.Agents.FirstOrDefault(x => x.Mbox_SHA1SUM == mboxSha1sum);
        }

        public AgentEntity GetByOpenID(string openid)
        {
            return context.Agents.FirstOrDefault(x => x.OpenId == openid);
        }

        public AgentEntity GetByAccount(string homePage, string name)
        {
            return context.Agents.FirstOrDefault(x => x.Account_HomePage == homePage && x.Account_Name == name);
        }

        public void Insert(AgentEntity agent)
        {
            context.Agents.Add(agent);
        }

        public AgentEntity GetAgent(AgentEntity agent)
        {
            if (string.IsNullOrEmpty(agent.Mbox))
            {
                return context.Agents.FirstOrDefault(x => x.Mbox == agent.Mbox);
            }

            if (string.IsNullOrEmpty(agent.Mbox_SHA1SUM))
            {
                return context.Agents.FirstOrDefault(x => x.Mbox_SHA1SUM == agent.Mbox_SHA1SUM);
            }

            if (string.IsNullOrEmpty(agent.OpenId))
            {
                return context.Agents.FirstOrDefault(x => x.OpenId == agent.OpenId);
            }

            if (string.IsNullOrEmpty(agent.Account_HomePage))
            {
                return context.Agents.FirstOrDefault(x => x.Account_HomePage == agent.Account_HomePage && x.Account_Name == agent.Account_Name);
            }
            return null;
        }
    }
}
