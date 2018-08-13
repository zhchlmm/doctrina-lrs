using System;
using System.Collections.Generic;
using Doctrina.Core.Data;
using Doctrina.Core.Data.Documents;
using Doctrina.xAPI.Models;

namespace Doctrina.Core.Repositories
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