using System;
using Doctrina.Core.Data;
using Doctrina.xAPI.Models;

namespace Doctrina.Core.Repositories
{
    public interface IActivityRepository
    {
        ActivityEntity GetByActivityId(string activityId);
        ActivityEntity GetByActivityId(Iri activityId);
        ActivityEntity Create(ActivityEntity entity);
        void Update(ActivityEntity entity);
    }
}