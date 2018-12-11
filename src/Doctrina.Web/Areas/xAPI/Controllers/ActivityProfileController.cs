using Doctrina.Core.Services;
using Doctrina.Web.Areas.xAPI.Mvc.Filters;
using Doctrina.xAPI;
using Doctrina.xAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Doctrina.Web.Areas.xAPI.Controllers
{
    [VersionHeader]
    [Route("xapi/activities/profile")]
    [Produces("application/json")]
    public class ActivityProfileController : ApiControllerBase
    {
        private readonly IActivityProfileService profileService;

        public ActivityProfileController(IActivityProfileService activityProfileService)
        {
            this.profileService = activityProfileService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId">The Activity id associated with this Profile document.</param>
        /// <param name="profileId">The profile id associated with this Profile document.</param>
        /// <returns>200 OK, the Profile document</returns>
        [HttpGet]
        public IActionResult GetDocument(Iri activityId, string profileId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var profile = this.profileService.GetActivityProfile(profileId, activityId);
                var document = profile.Document;
                string lastModified = document.Timestamp.ToString(Constants.Formats.DateTimeFormat);
                // TODO: Implement concurrency

                Response.ContentType = document.ContentType;
                Response.Headers.Add("LastModified", lastModified);
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return new FileContentResult(document.Content, document.ContentType);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Fetches Profile ids of all Profile documents for an Activity.
        /// </summary>
        /// <param name="activityId">The Activity id associated with these Profile documents.</param>
        /// <param name="since">Only ids of Profile documents stored since the specified Timestamp (exclusive) are returned.</param>
        /// <returns>200 OK, Array of Profile id(s)</returns>
        [HttpGet]
        public ActionResult<Guid[]> GetMultipleDocuments(Iri activityId, DateTimeOffset? since = null)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var documents = this.profileService.GetActivityProfileDocuments(activityId, since);
                if (documents == null)
                    return Ok(new Guid[] { });

                IEnumerable<Guid> ids = documents.Select(x => x.Id);
                string lastModified = documents.OrderByDescending(x => x.LastModified).FirstOrDefault().LastModified.ToString(Constants.Formats.DateTimeFormat);

                Response.Headers.Add("LastModified", lastModified);
                return Ok(ids);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Stores or changes the specified Profile document in the context of the specified Activity.
        /// </summary>
        /// <param name="activityId">The Activity id associated with this Profile document.</param>
        /// <param name="profileId">The profile id associated with this Profile document.</param>
        /// <param name="document">The document to be stored or updated.</param>
        /// <returns>204 No Content</returns>
        [AcceptVerbs("PUT", "POST")]
        public IActionResult PostDocument(Iri activityId, string profileId, [FromBody]byte[] content, Guid? registration = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string contentType = Request.ContentType;
            try
            {
                // TODO: Implement concurrency
                var profile = profileService.CreateActivityProfile(
                    profileId,
                    activityId,
                    registration,
                    content,
                    contentType
                 );

                Response.Headers["ETag"] = profile.Document.ETag;

                return NoContent();
            }
            catch (Exception ex)
            {
                // TODO: If exception is by ETagMatchException
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Deletes the specified Profile document in the context of the specified Activity.
        /// </summary>
        /// <param name="activityId">The Activity id associated with this Profile document.</param>
        /// <param name="profileId">The profile id associated with this Profile document.</param>
        /// <returns>204 No Content</returns>
        [HttpDelete]
        public IActionResult DeleteDocument(string profileId, Iri activityId)
        {
            try
            {
                var profile = this.profileService.GetActivityProfile(profileId, activityId);
                if (profile == null)
                    return NotFound();

                this.profileService.DeleteProfile(profile);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
