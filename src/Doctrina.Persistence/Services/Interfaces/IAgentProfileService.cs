using Doctrina.Domain.Entities;
using Doctrina.Domain.Entities.Documents;
using Doctrina.xAPI;
using System;
using System.Collections.Generic;

namespace Doctrina.Persistence.Services
{
    public interface IAgentProfileService
    {
        IDocumentEntity MergeAgentProfile(Agent agent, string profileId, byte[] content, string contentType);
        void DeleteProfile(IAgentProfileEntity id);
        IAgentProfileEntity GetAgentProfile(Agent agent, string profileId);
        IEnumerable<IDocumentEntity> GetProfiles(Agent agent, DateTimeOffset? since = null);
    }
}