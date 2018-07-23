using Doctrina.Core;
using Doctrina.Core.Persistence.Extensions;
using Doctrina.Core.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using xAPI.Core.Models;

namespace Doctrina.Core.Repositories
{
    public class AgentsProfileRepository : IAgentProfileRepository
    {
        private readonly DoctrinaDbContext dbContext;

        public AgentsProfileRepository(DoctrinaDbContext context)
        {
            this.dbContext = context;
        }

        public AgentProfileEntity GetProfile(AgentEntity agent, string profileId)
        {
            var sql = this.dbContext.AgentProfiles.WhereAgent(agent)
                .FirstOrDefault(x => x.ProfileId == profileId);

            return sql;
        }

        public IEnumerable<AgentProfileEntity> GetProfiles(AgentEntity agent, DateTime? since = null)
        {
            var sql = this.dbContext.AgentProfiles.WhereAgent(agent);

            if (since.HasValue)
            {
                DateTime sinceDate = since.Value;
                sql.Where(x => x.Document.Timestamp >= sinceDate);
            }
            sql.OrderByDescending(x => x.Document.Timestamp);

            return sql;
        }

        public void Delete(Guid id)
        {
            var profile = this.dbContext.AgentProfiles.Find(id);
            this.dbContext.AgentProfiles.Remove(profile);
        }

        public void Create(AgentProfileEntity profile)
        {
            dbContext.AgentProfiles.Add(profile);
            dbContext.SaveChanges();
        }

        public void Update(AgentProfileEntity profile)
        {
            dbContext.AgentProfiles.Update(profile);
        }
    }
}
