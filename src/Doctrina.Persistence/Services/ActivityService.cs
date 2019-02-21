using Doctrina.Domain.Entities;
using Doctrina.Persistence.Repositories;
using Doctrina.xAPI;
using Doctrina.xAPI.Helpers;
using Newtonsoft.Json;
using System;

namespace Doctrina.Persistence.Services
{
    public class ActivityService : IActivityService
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

        public ActivityEntity MergeActivity(Iri activityId)
        {
            return MergeActivity(new Activity() { Id = activityId });
        }

        public ActivityEntity MergeActivity(Activity activity)
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
                Id = activity.Id.ToString(),
                ActivityId = SHAHelper.ComputeHash(activity.Id.ToString()),
                //Definition = new ActivityDefinition()
                //{
                //    Description = activity.Definition
                //}
                CanonicalData = activity.Definition?.ToJson(),
                // TODO: Activity Authority
                // Authority = ??
            };



            this.activities.Create(entity);

            return entity;
        }

        public Activity GetActivity(Iri activityId)
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
                Id = new Iri(entity.ActivityId)
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
