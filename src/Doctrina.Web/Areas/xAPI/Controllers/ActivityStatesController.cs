using Doctrina.Core.Services;
using Doctrina.Web.Areas.xAPI.Models;
using Doctrina.Web.Areas.xAPI.Mvc.Filters;
using Doctrina.xAPI;
using Doctrina.xAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Doctrina.Web.Areas.xAPI.Controllers
{
    /// <summary>
    /// Generally, this is a scratch area for Learning Record Providers that do not have their own internal storage, or need to persist state across devices.
    /// </summary>
    //[ApiAuthortize]
    //[ApiVersion]
    [VersionHeader]
    [Route("xapi/activities/state")]
    public class ActivityStatesController : ApiControllerBase
    {
        private readonly IActivityStateService activityStateService;

        protected ActivityStatesController(IActivityStateService activityStateService)
        {
            this.activityStateService = activityStateService;
        }

        // GET xapi/activities/state
        [HttpGet]
        public ActionResult<StateDocumentModel> GetSingleState(StateDocumentModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var stateDocument = activityStateService.GetStateDocument(model.StateId, model.ActivityId, model.Agent, model.Registration);
                if (stateDocument == null)
                    return new NotFoundResult();

                var contentType = new MediaTypeHeaderValue(stateDocument.ContentType);
                var content = new FileContentResult(stateDocument.Content, contentType);
                content.LastModified = stateDocument.LastModified;
                Response.Headers.Add("ETag", "\"" + stateDocument.ETag + "\"");
                return content;
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT|POST xapi/activities/state
        [AcceptVerbs("PUT", "POST")]
        public IActionResult PostSingleState(StateDocumentModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var state = activityStateService.MergeStateDocument(model.StateId, model.ActivityId, model.Agent, model.Registration, model.ContentType, model.Content);
                var etag = EntityTagHeaderValue.Parse(state.ETag);
                Response.Headers.Add("ETag", etag.ToString());
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        // DELETE xapi/activities/state
        [HttpDelete]
        public IActionResult DeleteSingleState([FromQuery]StateDocumentModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                activityStateService.DeleteState(model.StateId, model.ActivityId, model.Agent, model.Registration);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Fetches State ids of all state data for this context (Activity + Agent [ + registration if specified]). If "since" parameter is specified, this is limited to entries that have been stored or updated since the specified timestamp (exclusive).
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="strAgent"></param>
        /// <param name="stateId"></param>
        /// <param name="registration"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMutipleStates(Iri activityId, [FromQuery(Name = "agent")]string strAgent, Guid? registration = null, DateTime? since = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Agent agent = Agent.Parse(strAgent);

                var states = activityStateService.GetStates(activityId, agent, registration, since);

                IEnumerable<Guid> ids = states.Select(x => x.Id);
                string lastModified = states.OrderByDescending(x => x.LastModified).FirstOrDefault().LastModified.ToString(Constants.Formats.DateTimeFormat);
                Response.Headers.Add("LastModified", lastModified);

                return Ok(ids);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
