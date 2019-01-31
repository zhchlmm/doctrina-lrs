using Doctrina.Persistence.Entities;
using Doctrina.Persistence.Entities.Documents;
using Doctrina.Persistence.Repositories;
using Doctrina.xAPI;
using System;
using System.Collections.Generic;

namespace Doctrina.Persistence.Services
{
    public class ActivityProfileService : IActivityProfileService
    {
        private readonly DoctrinaDbContext _context;
        private readonly IActivityProfileRepository activityProfileRepository;
        private readonly IActivityService _activityService;
        private readonly IDocumentService documentService;

        public ActivityProfileService(DoctrinaDbContext context, IActivityProfileRepository activityProfileRepository, IActivityService activityService, IDocumentService documentService)
        {
            _context = context;
            this.activityProfileRepository = activityProfileRepository;
            _activityService = activityService;
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
        public IActivityProfileEntity CreateActivityProfile(string profileId, Iri activityId, Guid? registration, byte[] content, string contentType)
        {
            var profile = this.activityProfileRepository.GetProfile(activityId, profileId, registration);

            if (profile == null)
                return CreateProfile(activityId, profileId, registration, content, contentType);

            return UpdateProfile(profile, content, contentType);
        }

        public IActivityProfileEntity GetActivityProfile(string profileId, Iri activityId, Guid? registration = null)
        {
            return this.activityProfileRepository.GetProfile(activityId, profileId, registration);
        }

        private IActivityProfileEntity CreateProfile(Iri activityId, string profileId, Guid? registration, byte[] content, string contentType)
        {
            var doc = this.documentService.CreateDocument(contentType, content);
            var activity = this._activityService.MergeActivity(activityId);

            var profile = new ActivityProfileEntity()
            {
                Key = Guid.NewGuid(),
                ActivityKey = activity.Key,
                Activity = activity,
                ProfileId = profileId,
                RegistrationId = registration,
                DocumentId = doc.Id,
                Document = (DocumentEntity)doc
            };

            _context.ActivityProfiles.Add(profile);
            _context.SaveChanges();

            return profile;
        }

        private IActivityProfileEntity UpdateProfile(ActivityProfileEntity profile, byte[] content, string contentType)
        {
            profile.UpdateDate = DateTime.UtcNow;

            documentService.UpdateDocument(profile.Document, contentType, content);

            this.activityProfileRepository.UpdateAndSave(profile);

            return profile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="since">This is limited to entries that have been stored or updated since the specified Timestamp</param>
        /// <returns></returns>
        public IEnumerable<IDocumentEntity> GetActivityProfileDocuments(Iri activityId, DateTimeOffset? since = null)
        {
            if (activityId == null)
                throw new ArgumentNullException("activityId");

            var documents = this.activityProfileRepository.GetProfilesDocuments(activityId, since);

            if (documents == null)
                return null;


            return documents;
        }

        /// <summary>
        /// Deletes profile and related document
        /// </summary>
        /// <param name="profile"></param>
        public void DeleteProfile(IActivityProfileEntity profile)
        {
            documentService.DeleteDocument(profile.Document);
            activityProfileRepository.DeleteAndSaveChanges((ActivityProfileEntity)profile);
        }
    }
}
