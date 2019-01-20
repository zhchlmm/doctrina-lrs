using Doctrina.Core.Services;
using Doctrina.Web.Areas.xAPI.Mvc.Filters;
using Doctrina.xAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Doctrina.Web.Areas.xAPI.Controllers
{
    [HeadWithoutBody]
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
        [AcceptVerbs("GET", "HEAD", Order = 1)]
        public IActionResult GetProfile([BindRequired]string profileId, [BindRequired]Iri activityId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var profile = this.profileService.GetActivityProfile(profileId, activityId);
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
        [AcceptVerbs("GET", "HEAD", Order = 2)]
        public ActionResult<Guid[]> GetProfiles(Iri activityId, DateTimeOffset? since = null)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var documents = this.profileService.GetActivityProfileDocuments(activityId, since);
                if (documents == null)
                    return Ok(new Guid[] { });

                IEnumerable<Guid> ids = documents.Select(x => x.Id);
                string lastModified = documents.OrderByDescending(x => x.LastModified)
                    .FirstOrDefault()
                    .LastModified.ToString("o");

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
        public IActionResult SaveProfile(string profileId, Iri activityId, [FromBody]byte[] document, Guid? registration = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string contentType = Request.ContentType;

            try
            {
                var profile = profileService.CreateActivityProfile(
                    profileId,
                    activityId,
                    registration,
                    document,
                    contentType
                 );

                Response.Headers["ETag"] = profile.Document.Tag;

                return NoContent();
            }
            catch (Exception ex)
            {
                // TODO: If exception is by ETagMatchException
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the specified Profile document in the context of the specified Activity.
        /// </summary>
        /// <param name="activityId">The Activity id associated with this Profile document.</param>
        /// <param name="profileId">The profile id associated with this Profile document.</param>
        /// <returns>204 No Content</returns>
        [HttpDelete]
        public IActionResult DeleteProfile(string profileId, Iri activityId)
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
