using Doctrina.Persistence.Entities;
using Doctrina.Persistence.Entities.Documents;
using Doctrina.xAPI;
using System;
using System.Collections.Generic;

namespace Doctrina.Persistence.Repositories
{
    public interface IActivityProfileRepository
    {
        void DeleteAndSaveChanges(ActivityProfileEntity profile);
        ActivityProfileEntity GetProfile(Iri activityId, string profileId, Guid? registration = null);
        IEnumerable<IDocumentEntity> GetProfilesDocuments(Iri activityId, DateTimeOffset? since = null);
        void AddAndSave(ActivityProfileEntity profile);
        void UpdateAndSave(ActivityProfileEntity profile);
    }
}