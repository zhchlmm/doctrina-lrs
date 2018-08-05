using System;
using System.Collections.Generic;
using Doctrina.Core.Data;
using Doctrina.xAPI.Models;

namespace Doctrina.Core.Services
{
    public interface IActivityStateService
    {
        void DeleteState(string stateId, Uri activityId, Agent agent, Guid? registration);
        void DeleteStates(Uri activityId, Agent agent, Guid? registration);
        IDocumentEntity GetStateDocument(string stateId, Uri activityId, Agent agent, Guid? registration);
        IEnumerable<IDocumentEntity> GetStates(Uri activityId, Agent agent, Guid? registration, DateTime? since);
        IDocumentEntity MergeStateDocument(string stateId, Uri activityId, Agent agent, Guid? registration, string contentType, byte[] content);
    }
}