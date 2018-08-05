using System;
using System.Collections.Generic;
using Doctrina.Core.Data;
using Doctrina.Core.Data.Documents;

namespace Doctrina.Core.Services
{
    public interface IActivityProfileService
    {
        IActivityProfileEntity GetActivityProfile(string profileId, Uri activityId, Guid? registration = null);
        IEnumerable<IDocumentEntity> GetActivityProfileDocuments(Uri activityId, DateTimeOffset? since = null);
        IActivityProfileEntity CreateActivityProfile(string profileId, Uri activityId, Guid? registration, byte[] content, string contentType);
        void DeleteProfile(IActivityProfileEntity profile);
    }
}