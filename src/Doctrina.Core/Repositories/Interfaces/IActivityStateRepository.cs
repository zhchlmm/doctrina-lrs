using Doctrina.Persistence.Entities;
using Doctrina.Persistence.Entities.Documents;
using Doctrina.xAPI;
using System;
using System.Collections.Generic;

namespace Doctrina.Persistence.Repositories
{
    public interface IActivitiesStateRepository
    {
        ActivityStateEntity GetState(string stateId, Iri activityId, AgentEntity agent, Guid? registration);
        void Save(ActivityStateEntity current);
        IEnumerable<DocumentEntity> GetStateDocuments(Iri activityId, AgentEntity agent, Guid? registration, DateTime? since);
        void Delete(string stateId, Iri activityId, AgentEntity agent, Guid? registration);
        void Delete(ActivityStateEntity entity);
        void Delete(Iri activityId, AgentEntity agent, Guid? registration);
        ActivityStateEntity Create(ActivityStateEntity actvityState);
    }
}