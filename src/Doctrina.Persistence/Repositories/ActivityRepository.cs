using Doctrina.Domain.Entities;
using Doctrina.xAPI;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Doctrina.Persistence.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly DoctrinaDbContext context;

        public ActivityRepository(DoctrinaDbContext context)
        {
            this.context = context;
        }

        public ActivityEntity GetByActivityId(Iri activityId)
        {
            return GetByActivityId(activityId.ToString());
        }

        public ActivityEntity GetByActivityId(string activityId)
        {
            return context.Activities.FirstOrDefault(x => x.Id == activityId);
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
