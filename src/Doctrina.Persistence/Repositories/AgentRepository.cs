﻿using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Doctrina.Persistence.Repositories
{
    public class AgentRepository : IAgentRepository
    {
        private readonly DoctrinaDbContext context;

        public AgentRepository(DoctrinaDbContext context) 
        {
            this.context = context;
        }

        //public AgentEntity GetByMbox(string mbox)
        //{
        //    return context.Agents.FirstOrDefault(x => x.Mbox == mbox);
        //}

        //public AgentEntity GetByMboxSha1sum(string mboxSha1sum)
        //{
        //    return context.Agents.FirstOrDefault(x => x.Mbox_SHA1SUM == mboxSha1sum);
        //}

        //public AgentEntity GetByOpenID(string openid)
        //{
        //    return context.Agents.FirstOrDefault(x => x.OpenId == openid);
        //}

        //public AgentEntity GetByAccount(string homePage, string name)
        //{
        //    return context.Agents.FirstOrDefault(x => x.Account_HomePage == homePage && x.Account_Name == name);
        //}

        public void Insert(AgentEntity agent)
        {
            context.Agents.Add(agent);
            context.Entry(agent).State = EntityState.Added;
        }

        /// <summary>
        /// Gets agent or group
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public AgentEntity GetAgentOrGroup(AgentEntity agent)
        {
            var query = context.Agents.Where(x => x.ObjectType == agent.ObjectType);

            if (string.IsNullOrEmpty(agent.Mbox))
            {
                return query.FirstOrDefault(x => x.Mbox == agent.Mbox);
            }

            if (string.IsNullOrEmpty(agent.Mbox_SHA1SUM))
            {
                return query.FirstOrDefault(x => x.Mbox_SHA1SUM == agent.Mbox_SHA1SUM);
            }

            if (string.IsNullOrEmpty(agent.OpenId))
            {
                return query.FirstOrDefault(x => x.OpenId == agent.OpenId);
            }

            if (string.IsNullOrEmpty(agent.Account.HomePage))
            {
                return query.FirstOrDefault(x => x.Account.HomePage == agent.Account.HomePage && x.Account.Name == agent.Account.Name);
            }
            return null;
        }
    }
}