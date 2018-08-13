using Doctrina.Core.Data;
using Doctrina.Core.Data.Documents;
using Doctrina.xAPI.Models;
using System;
using System.Collections.Generic;

namespace Doctrina.Core.Repositories
{
    public interface IActivityStateRepository
    {
        ActivityStateEntity GetState(string stateId, Iri activityId, AgentEntity agent, Guid? registration);
        void Save(ActivityStateEntity current);
        IEnumerable<DocumentEntity> GetStateDocuments(Iri activityId, AgentEntity agent, Guid? registration, DateTime? since);
        void Delete(string stateId, Iri activityId, AgentEntity agent, Guid? registration);
        void Delete(Iri activityId, AgentEntity agent, Guid? registration);
        ActivityStateEntity Create(ActivityStateEntity actvityState);
    }
}