using Doctrina.Core.Persistence.Models;
using Doctrina.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Doctrina.xAPI.Documents;
using Doctrina.xAPI.Models;

namespace Doctrina.Core.Services
{
    public sealed class ActivityStateService
    {
        private readonly DoctrinaDbContext dbContext;
        private readonly IActivityStateRepository activityStates;
        private readonly IAgentService agentService;
        private readonly IDocumentService documentService;

        //private readonly DocumentService documents;

        public ActivityStateService(DoctrinaDbContext dbContext, IActivityStateRepository activityStateRepository, IAgentService agentService, IDocumentService documentService)
        {
            this.dbContext = dbContext;
            this.activityStates = activityStateRepository;
            this.agentService = agentService;
            this.documentService = documentService;
        }

        public IDocumentEntity MergeStateDocument(string stateId, Uri activityId, Agent agent, Guid? registration, string contentType, byte[] content)
        {
            var agentEntity = this.agentService.ConvertFrom(agent);

            ActivityStateEntity current = this.activityStates.GetState(stateId, activityId, agentEntity, registration);

            if(current != null)
            {
                documentService.UpdateDocument(current.Document, contentType, content);
                return current.Document;
            }
            else
            {
                return CreateStateDocument(activityId, agent, stateId, registration, contentType, content);
            }
        }

        /// <summary>
        /// Creates a new state document and agent if it does not exist.
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="agent"></param>
        /// <param name="stateId"></param>
        /// <param name="registration"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        private IDocumentEntity CreateStateDocument(Uri activityId, Agent agent, string stateId, Guid? registration, string contentType, byte[] content)
        {
            var agentEntity = this.agentService.MergeAgent(agent);

            var activityState = new ActivityStateEntity()
            {
                StateId = stateId,
                ActivityId = activityId.ToString(),
                AgentId = agentEntity.Id,
                RegistrationId = registration
            };

            var document = (DocumentEntity)documentService.CreateDocument(contentType, content);
            activityState.Document = document;
            activityState.DocumentId = document.Id;

            this.activityStates.Create(activityState);

            this.dbContext.SaveChanges();

            return activityState.Document;
        }

        private IDocumentEntity UpdateStateDocument(ActivityStateEntity activityState, string contentType, byte[] content)
        {
            documentService.UpdateDocument(activityState.Document, contentType, content);
            this.dbContext.Entry(activityState).State = EntityState.Unchanged;
            this.dbContext.SaveChanges();

            return activityState.Document;
        }

        public IDocumentEntity GetStateDocument(string stateId, Uri activityId, Agent agent, Guid? registration)
        {
            var agentEntity = this.agentService.ConvertFrom(agent);

            var state = this.activityStates.GetState(stateId, activityId, agentEntity, registration);
            if (state == null)
                return null;

            return state.Document;
        }

        /// <summary>
        /// Get multiple states
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="agent"></param>
        /// <param name="registration"></param>
        /// <param name="since"></param>
        /// <returns>Array of State id(s), and Last modified date</returns>
        public IEnumerable<IDocumentEntity> GetStates(Uri activityId, Agent agent, Guid? registration, DateTime? since)
        {
            var agentEntity = this.agentService.ConvertFrom(agent);

            IEnumerable<DocumentEntity> states = this.activityStates.GetStateDocuments(activityId, agentEntity, registration, since);

            if(states == null || states.Count() == 0)
            {
                return null;
            }

            return states;
        }

        /// <summary>
        /// Deletes a single state
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="stateId"></param>
        /// <param name="agent"></param>
        /// <param name="registration"></param>
        public void DeleteState(Uri activityId, string stateId, Agent agent, Guid? registration)
        {
            var agentEntity = agentService.ConvertFrom(agent);
            this.activityStates.Delete(stateId, activityId, agentEntity, registration);
        }

        /// <summary>
        /// Deletes all states that match
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="agent"></param>
        /// <param name="registration"></param>
        internal void DeleteStates(Uri activityId, Agent agent, Guid? registration)
        {
            var agentEntity = agentService.ConvertFrom(agent);
            this.activityStates.Delete(activityId, agentEntity, registration);
        }
    }
}
