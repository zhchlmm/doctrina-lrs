using Doctrina.Core.Persistence.Models;
using Doctrina.Core.Repositories;
using Newtonsoft.Json;
using System;
using xAPI.Core.Models;

namespace Doctrina.Core.Services
{
    public class ActivityService
    {
        private readonly DoctrinaDbContext dbContext;
        private readonly IAgentService agentService;
        private readonly IActivityRepository activities;

        public ActivityService(DoctrinaDbContext dbContext, IAgentService agentService, IActivityRepository activityRepository)
        {
            this.dbContext = dbContext;
            this.agentService = agentService;
            this.activities = activityRepository;
        }

        internal ActivityEntity MergeActivity(Activity activity)
        {
            if (activity == null)
                throw new ArgumentNullException("activity");

            var current = this.activities.GetByActivityId(activity.Id);
            if(current != null)
            {
                // TODO: Update Definition?
                return current;
            }

            var entity = new ActivityEntity()
            {
                ActivityId = activity.Id.ToString(),
                CanonicalData = activity.Definition?.ToJson(),
                // TODO: Activity Authority
                // Authority = ??
            };

            this.activities.Create(entity);

            return entity;
        }

        public Activity GetActivity(Uri activityId)
        {
            var entity = this.activities.GetByActivityId(activityId);
            if (entity == null)
                return null;

            return ConvertFrom(entity);
        }

        //internal IEnumerable<Activity> GetActivities()
        //{
        //    return this.activities.Get().Select(x=> EntityToActivity(x)).ToList();
        //}

        private Activity ConvertFrom(ActivityEntity entity)
        {
            var activity = new Activity()
            {
                Id = new Uri(entity.ActivityId)
            };

            if (!string.IsNullOrEmpty(entity.CanonicalData))
            {
                var definition = JsonConvert.DeserializeObject<ActivityDefinition>(entity.CanonicalData);
                activity.Definition = definition;
            }
            return activity;
        }
    }
}
