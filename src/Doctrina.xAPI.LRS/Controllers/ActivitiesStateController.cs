using Doctrina.Persistence.Services;
using Doctrina.xAPI.LRS.Models;
using Doctrina.xAPI.LRS.Mvc.Filters;
using Doctrina.xAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using Doctrina.Application.ActivityStates.Commands;
using MediatR;
using Doctrina.Application.ActivityStates.Queries;
using System.Threading.Tasks;
using Doctrina.xAPI.Documents;
using Doctrina.Domain.Entities.Documents;

namespace Doctrina.xAPI.LRS.Controllers
{
    /// <summary>
    /// Generally, this is a scratch area for Learning Record Providers that do not have their own internal storage, or need to persist state across devices.
    /// </summary>
    //[ApiAuthortize]
    //[ApiVersion]
    [HeadWithoutBody]
    [RequiredVersionHeaderAttribute]
    [Route("xapi/activities/state")]
    public class ActivitiesStateController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IActivitiesStateService _activityStateService;

        public ActivitiesStateController(IMediator mediator, IActivitiesStateService activityStateService)
        {
            _mediator = mediator;
            _activityStateService = activityStateService;
        }

        // GET|HEAD xapi/activities/state
        [AcceptVerbs("GET", "HEAD")]
        public async Task<ActionResult<StateDocumentModel>> GetSingleState(StateDocumentModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                ActivityStateDocument stateDocument = await _mediator.Send(new GetActivityStateQuery()
                {
                    StateId = model.StateId,
                    ActivityId = model.ActivityId,
                    Agent = model.Agent,
                    Registration = model.Registration
                });
                if (stateDocument == null)
                    return NotFound();

                if (HttpMethods.IsHead(Request.Method))
                    return NoContent();

                var content = new FileContentResult(stateDocument.Content, stateDocument.ContentType);
                content.LastModified = stateDocument.LastModified;
                content.EntityTag = stateDocument.ETag;
                return content;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT|POST xapi/activities/state
        [ProducesResponseType(204)]
        [AcceptVerbs("PUT", "POST")]
        public async Task<IActionResult> PostSingleState(StateDocumentModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _mediator.Send(new MergeStateDocumentCommand()
                {
                    StateId = model.StateId,
                    ActivityId = model.ActivityId,
                    Agent = model.Agent,
                    Registration = model.Registration,
                    Content = model.Content,
                    ContentType = model.ContentType
                });

                //var etag = EntityTagHeaderValue.Parse($"\"{document.Tag}\"");
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
        public async Task<IActionResult> DeleteSingleState([FromQuery]StateDocumentModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                ActivityStateDocument stateDocument = await _mediator.Send(new GetActivityStateQuery()
                {
                    StateId = model.StateId,
                    ActivityId = model.ActivityId,
                    Agent = model.Agent,
                    Registration = model.Registration
                });
                if (stateDocument == null)
                {
                    return NotFound();
                }

                await _mediator.Send(new DeleteActivityStateCommand() {
                    StateId = model.StateId,
                    ActivityId = model.ActivityId,
                    Agent = model.Agent,
                    Registration = model.Registration
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE xapi/activities/state
        [HttpDelete]
        public async Task<IActionResult> DeleteStatesAsync([FromQuery]Iri activityId, [FromQuery(Name ="agent")]string strAgent, [FromQuery]Guid? registration = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Agent agent = Agent.Parse(strAgent);

                await _mediator.Send(new DeleteActivityStatesCommand()
                {
                    ActivityId = activityId,
                    Agent = agent,
                    Registration = registration
                });

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
        public async Task<IActionResult> GetMutipleStates(Iri activityId, [FromQuery(Name = "agent")]string strAgent, Guid? registration = null, DateTime? since = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // TODO: Parsing should happend at parameter level
                Agent agent = Agent.Parse(strAgent);

                ICollection<ActivityStateDocument> states = await _mediator.Send(new GetActivityStatesQuery()
                {
                    ActivityId = activityId,
                    Agent = agent,
                    Registration = registration,
                    Since = since
                });

                if(states.Count <= 0)
                {
                    return Ok(new string[0]);
                }

                IEnumerable<string> ids = states.Select(x => x.Id);
                string lastModified = states.OrderByDescending(x => x.LastModified)
                    .FirstOrDefault()?
                    .LastModified?.ToString("o");
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
