using Doctrina.Core.Persistence.Models;
using System;
using System.Collections.Generic;
using Doctrina.xAPI.Models;

namespace Doctrina.Core.Repositories
{
    public interface IActivityStateRepository
    {
        ActivityStateEntity GetState(string stateId, Uri activityId, AgentEntity agent, Guid? registration);
        void Save(ActivityStateEntity current);
        IEnumerable<DocumentEntity> GetStateDocuments(Uri activityId, AgentEntity agent, Guid? registration, DateTime? since);
        void Delete(string stateId, Uri activityId, AgentEntity agent, Guid? registration);
        void Delete(Uri activityId, AgentEntity agent, Guid? registration);
        ActivityStateEntity Create(ActivityStateEntity actvityState);
    }
}