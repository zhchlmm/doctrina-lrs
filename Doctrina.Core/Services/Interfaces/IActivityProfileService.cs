using System;
using System.Collections.Generic;
using Doctrina.Core.Persistence.Models;

namespace Doctrina.Core.Services
{
    public interface IActivityProfileService
    {
        IDocumentEntity GetProfile(Uri activityId, string profileId, Guid? registration = null);
        IEnumerable<ActivityProfileEntity> GetProfiles(Uri activityId, DateTimeOffset? since = null);
        ActivityProfileEntity SaveProfile(Uri activityId, string profileId, byte[] content, string contentType, Guid? registration = null);
    }
}