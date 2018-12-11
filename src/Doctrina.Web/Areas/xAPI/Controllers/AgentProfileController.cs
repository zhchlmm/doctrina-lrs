using Doctrina.Core.Services;
using Doctrina.Web.Areas.xAPI.Mvc.Filters;
using Doctrina.xAPI;
using Doctrina.xAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace Doctrina.Web.Areas.xAPI.Controllers
{
    [VersionHeader]
    [Route("xapi/agents/profile")]
    [Produces("application/json")]
    public class AgentProfileController : ApiControllerBase
    {
        private IAgentProfileService agentProfileService;

        public AgentProfileController(IAgentProfileService agentProfileService)
        {
            this.agentProfileService = agentProfileService;
        }

        [HttpGet]
        public ActionResult GetProfile(string profileId, [FromQuery(Name = "agent")]string strAgent)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Agent agent = Agent.Parse(strAgent);

                var profile = agentProfileService.GetAgentProfile(agent, profileId);
                if (profile == null)
                    return NotFound();

                var document = profile.Document;
                if (document == null)
                    return NotFound();

                // TODO: Implement Concurrency

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
        /// 
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="profileId"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        [AcceptVerbs("PUT", "POST")]
        public ActionResult StoreDocument(string profileId, [FromQuery(Name = "agent")]string strAgent, [FromBody]byte[] content)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                string contentType = Request.ContentType;
                Agent agent = Agent.Parse(strAgent);

                var document = agentProfileService.MergeAgentProfile(
                    agent,
                    profileId,
                    content,
                    contentType
                 );

                // TODO: Implement concurrency

                
                Response.Headers.Add("ETag", $"\"{document.ETag}\"");
                Response.Headers.Add("LastModified", document.LastModified.ToString("o"));
                return Ok();
            }
            catch (Exception ex)
            {
                // TODO: If exception is by ETagMatchException
                return BadRequest(ex);
            }
        }


        [HttpDelete]
        public ActionResult DeleteProfile(string profileId, [FromQuery(Name = "agent")]string strAgent,  [FromBody]byte[] content)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                string contentType = Request.ContentType;
                Agent agent = Agent.Parse(strAgent);

                var profile = agentProfileService.GetAgentProfile(agent, profileId);
                if (profile == null)
                    return NotFound();

                // TODO: Implement Concurrency

                agentProfileService.DeleteProfile(profile);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
