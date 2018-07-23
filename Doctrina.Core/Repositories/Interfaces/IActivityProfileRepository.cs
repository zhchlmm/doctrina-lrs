using System;
using System.Collections.Generic;
using Doctrina.Core.Persistence.Models;

namespace Doctrina.Core.Repositories
{
    public interface IActivityProfileRepository
    {
        void Delete(ActivityProfileEntity profile);
        ActivityProfileEntity GetProfile(Uri activityId, string profileId, Guid? registration = null);
        IEnumerable<ActivityProfileEntity> GetProfiles(Uri activityId, DateTimeOffset? since = null);
        void Insert(ActivityProfileEntity profile);
        void Update(ActivityProfileEntity profile);
    }
}