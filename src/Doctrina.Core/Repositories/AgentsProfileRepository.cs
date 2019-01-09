using Doctrina.Core.Data;
using Doctrina.Core.Data.Documents;
using Doctrina.Core.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Doctrina.Core.Repositories
{
    public class AgentProfileRepository : IAgentProfileRepository
    {
        private readonly DoctrinaContext dbContext;

        public AgentProfileRepository(DoctrinaContext context)
        {
            this.dbContext = context;
        }

        public void CreateAndSaveChanges(AgentProfileEntity profile)
        {
            dbContext.AgentProfiles.Add(profile);
            dbContext.SaveChanges();
        }

        public AgentProfileEntity GetProfile(AgentEntity agent, string profileId)
        {
            var sql = this.dbContext.AgentProfiles
                .Include(x=> x.Document)
                .WhereAgent(agent)
                .FirstOrDefault(x => x.ProfileId == profileId);

            return sql;
        }

        public IEnumerable<AgentProfileEntity> GetProfiles(AgentEntity agent, DateTimeOffset? since = null)
        {
            var sql = this.dbContext.AgentProfiles
                .Include(x=> x.Document)
                .WhereAgent(agent);

            if (since.HasValue)
            {
                sql.Where(x => x.Document.LastModified >= since.Value);
            }
            sql.OrderByDescending(x => x.Document.LastModified);

            return sql;
        }

        public void Update(AgentProfileEntity profile)
        {
            dbContext.AgentProfiles.Update(profile);
            dbContext.SaveChanges();
        }

        public void Delete(IAgentProfileEntity profile)
        {
            this.dbContext.AgentProfiles.Remove((AgentProfileEntity)profile);
            this.dbContext.SaveChanges();
            //this.dbContext.Entry(profile).State = EntityState.Deleted;
        }
    }
}
