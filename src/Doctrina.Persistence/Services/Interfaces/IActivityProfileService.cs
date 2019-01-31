using Doctrina.Domain.Entities.Documents;
using Doctrina.xAPI;
using System;
using System.Collections.Generic;

namespace Doctrina.Persistence.Services
{
    public interface IActivityProfileService
    {
        IActivityProfileEntity GetActivityProfile(string profileId, Iri activityId, Guid? registration = null);
        IEnumerable<IDocumentEntity> GetActivityProfileDocuments(Iri activityId, DateTimeOffset? since = null);
        IActivityProfileEntity CreateActivityProfile(string profileId, Iri activityId, Guid? registration, byte[] content, string contentType);
        void DeleteProfile(IActivityProfileEntity profile);
    }
}