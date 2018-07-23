using Doctrina.Core.Persistence.Models;
using Doctrina.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using xAPI.Core.Documents;
using xAPI.Core.Models;

namespace Doctrina.Core.Services
{
    public class AgentsProfileService
    {
        private readonly IAgentProfileRepository agentProfiles;
        private readonly IDocumentService documentService;
        private readonly IAgentService agentService;

        public AgentsProfileService(DoctrinaDbContext dbContext, IAgentProfileRepository agentProfiles, IDocumentService documentService, IAgentService agentService)
        {
            this.agentProfiles = agentProfiles;
            this.documentService = documentService;
            this.agentService = agentService;
        }

        public IDocumentEntity GetProfile(Agent agent, string profileId)
        {
            var agentEntity = agentService.ConvertFrom(agent);
            var profile = this.agentProfiles.GetProfile(agentEntity, profileId);
            if(profile == null)
            {
                return null;
            }
            return profile.Document;
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
        public IDocumentEntity CreateAgentProfile(Agent agent, string profileId, Document doc)
        {
            var agentEntity = agentService.ConvertFrom(agent);
            var profile = this.agentProfiles.GetProfile(agentEntity, profileId);
            if (profile == null)
            {
                // Is new
                return CreateNewAgentProfile(agent, profileId, doc);
            }
            return UpdateProfile(profile, doc);
        }

        private IDocumentEntity CreateNewAgentProfile(Agent agent, string profileId, Document doc)
        {
            var agentEntity = this.agentService.MergeAgent(agent);

            var profile = new AgentProfileEntity()
            {
                AgentId = agentEntity.Id,
                ProfileId = profileId,
            };

            var newDocument = documentService.CreateDocument(doc.ContentType, doc.Content);
            profile.DocumentId = newDocument.Id;

            this.agentProfiles.Create(profile);
            return profile.Document;
        }

        public void DeleteProfile(Guid id)
        {
            this.agentProfiles.Delete(id);
        }

        private IDocumentEntity UpdateProfile(AgentProfileEntity profile, Document doc)
        {
            documentService.UpdateDocument(profile.Document, doc);

            this.agentProfiles.Update(profile);

            return profile.Document;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="since">This is limited to entries that have been stored or updated since the specified Timestamp</param>
        /// <returns></returns>
        public IEnumerable<IDocumentEntity> GetProfiles(Agent agent, DateTime? since = null)
        {
            if (agent == null)
                throw new ArgumentNullException("agent");

            var agentEntity = agentService.ConvertFrom(agent);

            var profiles = this.agentProfiles.GetProfiles(agentEntity, since);

            if (profiles == null)
                return null;

            return profiles.Select(x=>x.Document);
        } 
    }
}
