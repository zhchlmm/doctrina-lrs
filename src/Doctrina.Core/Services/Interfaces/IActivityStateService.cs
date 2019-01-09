using Doctrina.Core.Data;
using Doctrina.Core.Data.Documents;
using Doctrina.xAPI;
using Doctrina.xAPI.Models;
using System;
using System.Collections.Generic;

namespace Doctrina.Core.Services
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