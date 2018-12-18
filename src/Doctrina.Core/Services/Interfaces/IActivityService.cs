using Doctrina.Core.Data;
using Doctrina.xAPI;
using Doctrina.xAPI.Models;

namespace Doctrina.Core.Services
{
    public interface IActivityService
    {
        ActivityEntity MergeActivity(Iri activityId);
        ActivityEntity MergeActivity(Activity target);
        Activity GetActivity(Iri activityId);
    }
}
