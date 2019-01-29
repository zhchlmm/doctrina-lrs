using Doctrina.Persistence.Entities;
using Doctrina.xAPI;

namespace Doctrina.Persistence.Services
{
    public interface IActivityService
    {
        ActivityEntity MergeActivity(Iri activityId);
        ActivityEntity MergeActivity(Activity target);
        Activity GetActivity(Iri activityId);
    }
}
