using Doctrina.Persistence.Services;
using Doctrina.xAPI.LRS.Mvc.Filters;
using Doctrina.xAPI;
using Microsoft.AspNetCore.Mvc;

namespace Doctrina.xAPI.LRS.Controllers
{
    [HeadWithoutBody]
    [RequiredVersionHeaderAttribute]
    [Route("xapi/activities")]
    [Produces("application/json")]
    public class ActivitiesController : ApiControllerBase
    {
        private readonly IActivityService activityService;

        public ActivitiesController(IActivityService activityService)
        {
            this.activityService = activityService;
        }

        [AcceptVerbs("GET", "HEAD")]
        public ActionResult GetActivityDocument(Iri activityId)
        {
            Activity activity = activityService.GetActivity(activityId);
            if (activity == null)
                return Ok(new Activity());

            // TODO: Return only canonical that match accept-language header, or und

            return Ok(activity);
        }
    }
}
