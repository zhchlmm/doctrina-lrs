using Doctrina.Core.Data;
using Doctrina.Core.Data.Documents;
using Doctrina.Core.Repositories;
using Doctrina.xAPI;
using Doctrina.xAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Doctrina.Core.Services
{
    public sealed class ActivitiesStateService : IActivitiesStateService
    {
        private readonly DoctrinaContext _dbContext;
        private readonly IActivitiesStateRepository _activityStates;
        private readonly IActivityService _activityService;
        private readonly IAgentService _agentService;
        private readonly IDocumentService _documentService;

        //private readonly DocumentService documents;

        public ActivitiesStateService(DoctrinaContext dbContext, IActivitiesStateRepository activityStateRepository, IActivityService activityService, IAgentService agentService, IDocumentService documentService)
        {
            _dbContext = dbContext;
            _activityStates = activityStateRepository;
            _activityService = activityService;
            _agentService = agentService;
            _documentService = documentService;
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

            _dbContext.ActivityStates.Add(activityState);
            _dbContext.Entry(activityState).State = EntityState.Added;
            _dbContext.SaveChanges();

            return activityState.Document;
        }

        private IDocumentEntity UpdateStateDocument(ActivityStateEntity activityState, string contentType, byte[] content)
        {
            var document = _documentService.UpdateDocument(activityState.Document, contentType, content);

            _dbContext.Entry(activityState).State = EntityState.Unchanged;
            _dbContext.SaveChanges();

            return document;
        }

        public ActivityStateEntity GetActivityState(string stateId, Iri activityId, Agent agent, Guid? registration)
        {
            var agentEntity = this._agentService.ConvertFrom(agent);

            var state = this._activityStates.GetState(stateId, activityId, agentEntity, registration);
            if (state == null)
                return null;

            return state;
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
        public void DeleteState(ActivityStateEntity state)
        {
            this._activityStates.Delete(state);
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
