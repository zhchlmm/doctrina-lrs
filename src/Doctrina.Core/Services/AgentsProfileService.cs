using Doctrina.Core.Data;
using Doctrina.Core.Data.Documents;
using Doctrina.Core.Repositories;
using Doctrina.xAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Doctrina.Core.Services
{
    public class AgentProfileService : IAgentProfileService
    {
        private readonly IAgentProfileRepository agentProfiles;
        private readonly IDocumentService documentService;
        private readonly IAgentService agentService;

        public AgentProfileService(DoctrinaContext dbContext, IAgentProfileRepository agentProfiles, IDocumentService documentService, IAgentService agentService)
        {
            this.agentProfiles = agentProfiles;
            this.documentService = documentService;
            this.agentService = agentService;
        }

        /// <summary>
        /// Stored or changes the specified Profile document in the context of the specified Activity.
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="profileId"></param>
        /// <param name="contents"></param>
        /// <param name="contentType"></param>
        /// <param name="etag"></param>
        /// <param name="eTagMatch"></param>
        /// <returns></returns>
        public IDocumentEntity MergeAgentProfile(Agent agent, string profileId, byte[] content, string contentType)
        {
            var agentEntity = agentService.ConvertFrom(agent);
            var profile = this.agentProfiles.GetProfile(agentEntity, profileId);
            if (profile == null)
            {
                // Is new
                return CreateNewAgentProfile(agent, profileId, content, contentType);
            }
            return UpdateProfile(profile, content, contentType);
        }

        private IDocumentEntity CreateNewAgentProfile(Agent agent, string profileId, byte[] content, string contentType)
        {
            var agentEntity = this.agentService.MergeActor(agent);

            var profile = new AgentProfileEntity()
            {
                AgentId = agentEntity.Key,
                ProfileId = profileId,
            };

            var newDocument = documentService.CreateDocument(contentType, content);
            profile.DocumentId = newDocument.Id;

            this.agentProfiles.CreateAndSaveChanges(profile);
            return profile.Document;
        }

        public IAgentProfileEntity GetAgentProfile(Agent agent, string profileId)
        {
            var agentEntity = agentService.ConvertFrom(agent);
            var profile = this.agentProfiles.GetProfile(agentEntity, profileId);
            if(profile == null)
            {
                return null;
            }
            return profile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="since">This is limited to entries that have been stored or updated since the specified Timestamp</param>
        /// <returns></returns>
        public IEnumerable<IDocumentEntity> GetProfiles(Agent agent, DateTimeOffset? since = null)
        {
            if (agent == null)
                throw new ArgumentNullException("agent");

            var agentEntity = agentService.ConvertFrom(agent);

            var profiles = this.agentProfiles.GetProfiles(agentEntity, since);

            if (profiles == null)
                return null;

            return profiles.Select(x => x.Document);
        }

        private IDocumentEntity UpdateProfile(AgentProfileEntity profile, byte[] content, string contentType)
        {
            documentService.UpdateDocument(profile.Document,contentType, content);

            this.agentProfiles.Update(profile);

            return profile.Document;
        }

        public void DeleteProfile(IAgentProfileEntity entity)
        {
            this.agentProfiles.Delete(entity);
        }
    }
}
