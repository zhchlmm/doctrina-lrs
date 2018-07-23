using Doctrina.Core.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Doctrina.Core.Repositories
{
    public class ActivityProfileRepository : IActivityProfileRepository
    {
        private readonly DoctrinaDbContext _context;

        public ActivityProfileRepository(DoctrinaDbContext context)
        {
            this._context = context;
        }

        public ActivityProfileEntity GetProfile(Uri activityId, string profileId, Guid? registragion = null)
        {

            string strActivityId = activityId.ToString();

            return _context.ActivityProfiles.FirstOrDefault(x => x.ActivityId == strActivityId && x.ProfileId == profileId && x.RegistrationId == registragion);
        }

        public ActivityProfileEntity GetProfile(Uri activityId, string profileId)
        {

            string strActivityId = activityId.ToString();

            return _context.ActivityProfiles.FirstOrDefault(x => x.ActivityId == strActivityId && x.ProfileId == profileId);
        }


        public IEnumerable<ActivityProfileEntity> GetProfiles(Uri activityId, DateTimeOffset? since = null)
        {
            string strActivityId = activityId.ToString();

            var query = _context.ActivityProfiles.Where(x => x.ActivityId == strActivityId);

            if (since.HasValue)
            {
                var sinceDate = since.Value;
                query.Where(x => x.UpdateDate >= sinceDate);
            }

            query.OrderByDescending(x => x.UpdateDate);

            return query.AsEnumerable();
        }

        /// <summary>
        /// Delete a single state document
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="agent"></param>
        /// <param name="stateId"></param>
        /// <param name="registration"></param>
        public void Delete(ActivityProfileEntity profile)
        {
            _context.ActivityProfiles.Remove(profile);
            _context.SaveChanges();
        }

        public void Insert(ActivityProfileEntity profile)
        {
            _context.ActivityProfiles.Add(profile);
            _context.SaveChanges();
        }

        public void Update(ActivityProfileEntity profile)
        {
            _context.ActivityProfiles.Update(profile);
            _context.SaveChanges();
        }
    }
}
