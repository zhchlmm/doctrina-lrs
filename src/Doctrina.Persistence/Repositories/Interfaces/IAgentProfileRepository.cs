using Doctrina.Domain.Entities;
using Doctrina.Domain.Entities.Documents;
using System;
using System.Collections.Generic;

namespace Doctrina.Persistence.Repositories
{
    public interface IAgentProfileRepository
    {
        AgentProfileEntity GetProfile(AgentEntity agentEntity, string profileId);
        IEnumerable<AgentProfileEntity> GetProfiles(AgentEntity agentEntity, DateTimeOffset? since);
        void CreateAndSaveChanges(AgentProfileEntity profile);
        void Update(AgentProfileEntity profile);
        void Delete(IAgentProfileEntity id);
    }
}