using Doctrina.Persistence.Services;
using Doctrina.xAPI.LRS.Mvc.Filters;
using Doctrina.xAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using Doctrina.Application.ActivityProfiles.Queries;
using System.Threading.Tasks;
using Doctrina.Application.ActivityProfiles.Commands;

namespace Doctrina.xAPI.LRS.Controllers
{
    [HeadWithoutBody]
    [RequiredVersionHeaderAttribute]
    [Route("xapi/activities/profile")]
    [Produces("application/json")]
    public class ActivityProfileController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ActivityProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId">The Activity id associated with this Profile document.</param>
        /// <param name="profileId">The profile id associated with this Profile document.</param>
        /// <returns>200 OK, the Profile document</returns>
        [AcceptVerbs("GET", "HEAD", Order = 1)]
        public async Task<IActionResult> GetProfile([BindRequired]string profileId, [BindRequired]Iri activityId, Guid? registration = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var profile = await _mediator.Send(new GetActivityProfileQuery()
            {
                ProfileId = profileId,
                ActivityId = activityId,
                Registration = registration
            });

            if (profile == null)
                return NotFound();

            var document = profile.Document;
            string lastModified = document.LastModified.ToString("o");
            // TODO: Implement concurrency

            Response.ContentType = document.ContentType;
            Response.Headers.Add("LastModified", lastModified);
            Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            return new FileContentResult(document.Content, document.ContentType);
        }

        /// <summary>
        /// Fetches Profile ids of all Profile documents for an Activity.
        /// </summary>
        /// <param name="activityId">The Activity id associated with these Profile documents.</param>
        /// <param name="since">Only ids of Profile documents stored since the specified Timestamp (exclusive) are returned.</param>
        /// <returns>200 OK, Array of Profile id(s)</returns>
        [AcceptVerbs("GET", "HEAD", Order = 2)]
        public async Task<ActionResult<Guid[]>> GetProfiles(Iri activityId, DateTimeOffset? since = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var profiles = await _mediator.Send(new GetActivityProfilesQuery()
            {
                ActivityId = activityId,
                Since = since
            });

            if (profiles == null)
                return Ok(new Guid[0]);

            IEnumerable<Guid> ids = profiles.Select(x => x.Key);
            string lastModified = profiles.OrderByDescending(x => x.Document.LastModified)
                .FirstOrDefault()
                .Document
                .LastModified.ToString("o");

            Response.Headers.Add("LastModified", lastModified);
            return Ok(ids);
        }

        /// <summary>
        /// Stores or changes the specified Profile document in the context of the specified Activity.
        /// </summary>
        /// <param name="activityId">The Activity id associated with this Profile document.</param>
        /// <param name="profileId">The profile id associated with this Profile document.</param>
        /// <param name="document">The document to be stored or updated.</param>
        /// <returns>204 No Content</returns>
        [AcceptVerbs("PUT", "POST")]
        public async Task<IActionResult> SaveProfile(string profileId, Iri activityId, [FromBody]byte[] document, Guid? registration = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string contentType = Request.ContentType;

            var profile = await _mediator.Send(new CreateActivityProfileCommand()
            {
                ProfileId = profileId,
                ActivityId = activityId,
                Content = document,
                ContentType = contentType,
                Registration = registration
            });

            Response.Headers["ETag"] = profile.Document.Checksum;

            return NoContent();
        }

        /// <summary>
        /// Deletes the specified Profile document in the context of the specified Activity.
        /// </summary>
        /// <param name="activityId">The Activity id associated with this Profile document.</param>
        /// <param name="profileId">The profile id associated with this Profile document.</param>
        /// <returns>204 No Content</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteProfileAsync(string profileId, Iri activityId, Guid? registration = null)
        {
            try
            {
                var profile = _mediator.Send(new GetActivityProfileQuery()
                {
                    ProfileId = profileId,
                    ActivityId = activityId,
                    Registration = registration
                });

                if (profile == null)
                    return NotFound();

                await _mediator.Send(new DeleteActivityProfileCommand()
                {
                    ProfileId = profileId,
                    ActivityId = activityId,
                    Registration = registration
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
