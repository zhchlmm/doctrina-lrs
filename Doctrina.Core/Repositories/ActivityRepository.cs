using Doctrina.Core.Persistence.Models;
using System;
using System.Linq;

namespace Doctrina.Core.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly DoctrinaDbContext context;

        public ActivityRepository(DoctrinaDbContext context)
        {
            this.context = context;
        }

        public ActivityEntity GetByActivityId(Uri activityId)
        {
            return GetByActivityId(activityId.ToString());
        }

        public ActivityEntity GetByActivityId(string activityId)
        {
            return context.Activities.FirstOrDefault(x => x.ActivityId == activityId);
        }

        public ActivityEntity Create(ActivityEntity entity)
        {
            context.Activities.Add(entity);
            context.SaveChanges();
            return entity;
        }

        public void Update(ActivityEntity entity)
        {
            context.Activities.Update(entity);
            context.SaveChanges();
        }
    }
}
