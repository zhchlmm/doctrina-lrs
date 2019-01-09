using Doctrina.Core.Services;
using Doctrina.Web.Areas.xAPI.Models;
using Doctrina.Web.Areas.xAPI.Mvc.Filters;
using Doctrina.xAPI;
using Doctrina.xAPI.Models;
using Microsoft.AspNetCore.Http;
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
    [HeadWithoutBody]
    [VersionHeader]
    [Route("xapi/activities/state")]
    public class ActivitiesStateController : ApiControllerBase
    {
        private readonly IActivitiesStateService _activityStateService;

        public ActivitiesStateController(IActivitiesStateService activityStateService)
        {
            _activityStateService = activityStateService;
        }

        // GET|HEAD xapi/activities/state
        [AcceptVerbs("GET", "HEAD")]
        public ActionResult<StateDocumentModel> GetSingleState(StateDocumentModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var activityState = _activityStateService.GetActivityState(model.StateId, model.ActivityId, model.Agent, model.Registration);
                if (activityState == null)
                    return NotFound();

                if (HttpMethods.IsHead(Request.Method))
                    return NoContent();

                var stateDocument = activityState.Document;

                var contentType = new MediaTypeHeaderValue(stateDocument.ContentType);
                var content = new FileContentResult(stateDocument.Content, contentType.ToString());
                content.LastModified = stateDocument.LastModified;
                content.EntityTag = new EntityTagHeaderValue($"\"{stateDocument.Tag}\"");
                return content;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                var document = _activityStateService.MergeStateDocument(model.StateId, model.ActivityId, model.Agent, model.Registration, model.ContentType, model.Content);
                var etag = EntityTagHeaderValue.Parse($"\"{document.Tag}\"");
                //Response.Headers.Add("ETag", etag.ToString());
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                var state = _activityStateService.GetActivityState(model.StateId, model.ActivityId, model.Agent, model.Registration);
                if (state == null)
                    return NotFound();

                _activityStateService.DeleteState(state);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE xapi/activities/state
        [HttpDelete]
        public IActionResult DeleteStates([FromQuery]Iri activityId, [FromQuery(Name ="agent")]string strAgent, [FromQuery]Guid? registration = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Agent agent = Agent.Parse(strAgent);

                _activityStateService.DeleteStates(activityId, agent, registration);
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

                var states = _activityStateService.GetStates(activityId, agent, registration, since);

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
