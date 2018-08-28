using Doctrina.Core.Data;
using Doctrina.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Doctrina.xAPI.Documents;
using Doctrina.xAPI.Models;
using Doctrina.Core.Data.Documents;

namespace Doctrina.Core.Services
{
    public sealed class ActivityStateService : IActivityStateService
    {
        private readonly DoctrinaContext _dbContext;
        private readonly IActivityStateRepository _activityStates;
        private readonly IActivityService _activityService;
        private readonly IAgentService _agentService;
        private readonly IDocumentService _documentService;

        //private readonly DocumentService documents;

        public ActivityStateService(DoctrinaContext dbContext, IActivityStateRepository activityStateRepository, IActivityService activityService, IAgentService agentService, IDocumentService documentService)
        {
            this._dbContext = dbContext;
            this._activityStates = activityStateRepository;
            _activityService = activityService;
            this._agentService = agentService;
            this._documentService = documentService;
        }

        public IDocumentEntity MergeStateDocument(string stateId, Iri activityId, Agent agent, Guid? registration, string contentType, byte[] content)
        {
            var agentEntity = this._agentService.ConvertFrom(agent);

            ActivityStateEntity current = this._activityStates.GetState(stateId, activityId, agentEntity, registration);

            if(current != null)
            {
                _documentService.UpdateDocument(current.Document, contentType, content);
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
        private IDocumentEntity CreateStateDocument(Iri activityId, Agent agent, string stateId, Guid? registration, string contentType, byte[] content)
        {
            var agentEntity = this._agentService.MergeActor(agent);
            var activity = new Activity()
            {
                Id = activityId
            };
            var activityEntity = _activityService.MergeActivity(activity);

            var activityState = new ActivityStateEntity()
            {
                StateId = stateId,
                ActivityKey = activityEntity.Key,
                AgentId = agentEntity.Key,
                RegistrationId = registration
            };

            var document = (DocumentEntity)_documentService.CreateDocument(contentType, content);
            activityState.Document = document;
            activityState.DocumentId = document.Id;

            this._activityStates.Create(activityState);

            this._dbContext.SaveChanges();

            return activityState.Document;
        }

        private IDocumentEntity UpdateStateDocument(ActivityStateEntity activityState, string contentType, byte[] content)
        {
            _documentService.UpdateDocument(activityState.Document, contentType, content);
            this._dbContext.Entry(activityState).State = EntityState.Unchanged;
            this._dbContext.SaveChanges();

            return activityState.Document;
        }

        public IDocumentEntity GetStateDocument(string stateId, Iri activityId, Agent agent, Guid? registration)
        {
            var agentEntity = this._agentService.ConvertFrom(agent);

            var state = this._activityStates.GetState(stateId, activityId, agentEntity, registration);
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
        public IEnumerable<IDocumentEntity> GetStates(Iri activityId, Agent agent, Guid? registration, DateTime? since)
        {
            var agentEntity = this._agentService.ConvertFrom(agent);

            IEnumerable<DocumentEntity> states = this._activityStates.GetStateDocuments(activityId, agentEntity, registration, since);

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
        public void DeleteState(string stateId, Iri activityId, Agent agent, Guid? registration)
        {
            var agentEntity = _agentService.ConvertFrom(agent);
            this._activityStates.Delete(stateId, activityId, agentEntity, registration);
        }

        /// <summary>
        /// Deletes all states that match
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="agent"></param>
        /// <param name="registration"></param>
        public void DeleteStates(Iri activityId, Agent agent, Guid? registration)
        {
            var agentEntity = _agentService.ConvertFrom(agent);
            this._activityStates.Delete(activityId, agentEntity, registration);
        }
    }
}
