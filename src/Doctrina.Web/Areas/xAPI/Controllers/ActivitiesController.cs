using Doctrina.Core.Services;
using Doctrina.Web.Areas.xAPI.Mvc.Filters;
using Doctrina.xAPI;
using Microsoft.AspNetCore.Mvc;

namespace Doctrina.Web.Areas.xAPI.Controllers
{
    [HeadWithoutBody]
    [VersionHeader]
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
