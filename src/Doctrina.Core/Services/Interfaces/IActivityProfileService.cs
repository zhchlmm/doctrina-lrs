using System;
using System.Collections.Generic;
using Doctrina.Core.Data;
using Doctrina.Core.Data.Documents;
using Doctrina.xAPI.Models;

namespace Doctrina.Core.Services
{
    public interface IActivityProfileService
    {
        IActivityProfileEntity GetActivityProfile(string profileId, Iri activityId, Guid? registration = null);
        IEnumerable<IDocumentEntity> GetActivityProfileDocuments(Iri activityId, DateTimeOffset? since = null);
        IActivityProfileEntity CreateActivityProfile(string profileId, Iri activityId, Guid? registration, byte[] content, string contentType);
        void DeleteProfile(IActivityProfileEntity profile);
    }
}