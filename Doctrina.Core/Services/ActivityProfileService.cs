using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Doctrina.Core.Persistence.Models;
using Doctrina.Core.Repositories;

namespace Doctrina.Core.Services
{
    public class ActivityProfileService : IActivityProfileService
    {
        private readonly IActivityProfileRepository profiles;
        private readonly IDocumentService documentService;

        public ActivityProfileService(IActivityProfileRepository activityProfileRepository, IDocumentService documentService)
        {
            this.profiles = activityProfileRepository;
            this.documentService = documentService;
        }

        /// <summary>
        /// Stored or changes the specified Profile document in the context of the specified Activity.
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="profileId"></param>
        /// <param name="contents"></param>
        /// <param name="contentType"></param>
        /// <param name="etag"></param>
        /// <param name="eTagMatch"></param>
        /// <returns></returns>
        public ActivityProfileEntity SaveProfile(Uri activityId, string profileId, byte[] content, string contentType, Guid? registration = null)
        {
            var profile = this.profiles.GetProfile(activityId, profileId, registration);
            if (profile == null)
                return CreateProfile(activityId, profileId, content, contentType);
            return UpdateProfile(profile, content, contentType);
        }

        public IDocumentEntity GetProfile(Uri activityId, string profileId, Guid? registration = null)
        {
            var profile = this.profiles.GetProfile(activityId, profileId);
            return profile.Document;
        }

        private ActivityProfileEntity CreateProfile(Uri activityId, string profileId, byte[] content, string contentType, Guid? registration = null)
        {
            var doc = this.documentService.CreateDocument(contentType, content);

            var profile = new ActivityProfileEntity()
            {
                Key = Guid.NewGuid(),
                ActivityId = activityId.ToString(),
                ProfileId = profileId,
                RegistrationId = registration,
                DocumentId = doc.Id
            };

            this.profiles.Insert(profile);

            return profile;
        }

        private ActivityProfileEntity UpdateProfile(ActivityProfileEntity profile, byte[] content, string contentType)
        {
            profile.UpdateDate = DateTime.UtcNow;

            documentService.UpdateDocument(profile.Document, contentType, content);

            this.profiles.Update(profile);

            return profile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="since">This is limited to entries that have been stored or updated since the specified Timestamp</param>
        /// <returns></returns>
        public IEnumerable<ActivityProfileEntity> GetProfiles(Uri activityId, DateTimeOffset? since = null)
        {
            if (activityId == null)
                throw new ArgumentNullException("activityId");

            var profiles = this.profiles.GetProfiles(activityId, since);

            if (profiles == null)
                return null;

            //var model = new MultipleDocuments()
            //{
            //    Ids = profiles.Select(x => x.ProfileId),
            //    LastModified = profiles.First().UpdateDate
            //};

            return profiles;
        }

        public void DeleteProfile(IDocumentEntity profile)
        {
            throw new NotImplementedException();
            //this.con.Delete(profile.Id);
        }
    }
}
