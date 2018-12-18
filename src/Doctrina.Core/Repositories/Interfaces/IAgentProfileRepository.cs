using System;
using System.Collections.Generic;
using Doctrina.Core.Data;
using Doctrina.Core.Data.Documents;

namespace Doctrina.Core.Repositories
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