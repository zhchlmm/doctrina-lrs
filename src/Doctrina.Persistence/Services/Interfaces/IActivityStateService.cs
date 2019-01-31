using Doctrina.Domain.Entities.Documents;
using Doctrina.xAPI;
using System;
using System.Collections.Generic;

namespace Doctrina.Persistence.Services
{
    public interface IActivitiesStateService
    {
        void DeleteState(ActivityStateEntity document);
        void DeleteStates(Iri activityId, Agent agent, Guid? registration);
        ActivityStateEntity GetActivityState(string stateId, Iri activityId, Agent agent, Guid? registration);
        IEnumerable<IDocumentEntity> GetStates(Iri activityId, Agent agent, Guid? registration, DateTime? since);
        IDocumentEntity MergeStateDocument(string stateId, Iri activityId, Agent agent, Guid? registration, string contentType, byte[] content);
    }
}