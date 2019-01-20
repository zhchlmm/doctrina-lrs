using Doctrina.Core.Data;
using Doctrina.Core.Data.Documents;
using Doctrina.xAPI;
using System;
using System.Collections.Generic;

namespace Doctrina.Core.Services
{
    public interface IAgentProfileService
    {
        IDocumentEntity MergeAgentProfile(Agent agent, string profileId, byte[] content, string contentType);
        void DeleteProfile(IAgentProfileEntity id);
        IAgentProfileEntity GetAgentProfile(Agent agent, string profileId);
        IEnumerable<IDocumentEntity> GetProfiles(Agent agent, DateTimeOffset? since = null);
    }
}