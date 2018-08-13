using Doctrina.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Doctrina.xAPI.Models;
using Doctrina.Web.Areas.xAPI.Mvc.Filters;

namespace Doctrina.Web.Areas.xAPI.Controllers
{
    [VersionHeader]
    [Route("xapi/activities")]
    [Produces("application/json")]
    public class ActivitiesController : ApiControllerBase
    {
        private readonly IActivityService activityService;

        protected ActivitiesController(IActivityService activityService)
        {
            this.activityService = activityService;
        }

        [HttpGet]
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
