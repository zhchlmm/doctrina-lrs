using System;
using System.Collections.Generic;
using Doctrina.Core.Persistence.Models;

namespace Doctrina.Core.Repositories
{
    public interface IAgentProfileRepository
    {
        AgentProfileEntity GetProfile(AgentEntity agentEntity, string profileId);
        IEnumerable<AgentProfileEntity> GetProfiles(AgentEntity agentEntity, DateTime? since);
        void Create(AgentProfileEntity profile);
        void Update(AgentProfileEntity profile);
        void Delete(Guid id);
    }
}