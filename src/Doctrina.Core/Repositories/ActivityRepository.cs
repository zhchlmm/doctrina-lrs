using Doctrina.Core.Data;
using Doctrina.xAPI;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Doctrina.Core.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly DoctrinaContext context;

        public ActivityRepository(DoctrinaContext context)
        {
            this.context = context;
        }

        public ActivityEntity GetByActivityId(Iri activityId)
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
            context.Entry(entity).State = EntityState.Added;
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
