using System;
using System.Collections.Generic;
using Doctrina.Core.Data;
using Doctrina.Core.Data.Documents;

namespace Doctrina.Core.Repositories
{
    public interface IActivityProfileRepository
    {
        void DeleteAndSaveChanges(ActivityProfileEntity profile);
        ActivityProfileEntity GetProfile(Uri activityId, string profileId, Guid? registration = null);
        IEnumerable<IDocumentEntity> GetProfilesDocuments(Uri activityId, DateTimeOffset? since = null);
        void AddAndSave(ActivityProfileEntity profile);
        void UpdateAndSave(ActivityProfileEntity profile);
    }
}