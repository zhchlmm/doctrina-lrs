using Doctrina.Domain.Entities;
using Doctrina.xAPI;

namespace Doctrina.Persistence.Repositories
{
    public interface IActivityRepository
    {
        ActivityEntity GetByActivityId(string activityId);
        ActivityEntity GetByActivityId(Iri activityId);
        ActivityEntity Create(ActivityEntity entity);
        void Update(ActivityEntity entity);
    }
}