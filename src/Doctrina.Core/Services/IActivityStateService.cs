using System;
using System.Collections.Generic;
using Doctrina.Core.Data;
using Doctrina.xAPI.Models;

namespace Doctrina.Core.Services
{
    public interface IActivityStateService
    {
        void DeleteState(string stateId, Iri activityId, Agent agent, Guid? registration);
        void DeleteStates(Iri activityId, Agent agent, Guid? registration);
        IDocumentEntity GetStateDocument(string stateId, Iri activityId, Agent agent, Guid? registration);
        IEnumerable<IDocumentEntity> GetStates(Iri activityId, Agent agent, Guid? registration, DateTime? since);
        IDocumentEntity MergeStateDocument(string stateId, Iri activityId, Agent agent, Guid? registration, string contentType, byte[] content);
    }
}