using Doctrina.Persistence.Services;
using Doctrina.xAPI.LRS.Mvc.Filters;
using Doctrina.xAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Doctrina.xAPI.LRS.Controllers
{
    [HeadWithoutBody]
    [RequiredVersionHeader]
    [Route("xapi/agents/profile")]
    [Produces("application/json")]
    public class AgentProfileController : ApiControllerBase
    {
        private IAgentProfileService agentProfileService;

        public AgentProfileController(IAgentProfileService agentProfileService)
        {
            this.agentProfileService = agentProfileService;
        }

        [AcceptVerbs("GET", "HEAD", Order = 1)]
        public ActionResult GetAgentProfile(string profileId, [FromQuery(Name = "agent")]string strAgent)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(profileId))
                {
                    throw new ArgumentNullException(nameof(profileId));
                }

                if (string.IsNullOrWhiteSpace(strAgent))
                {
                    throw new ArgumentNullException("agent");
                }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Agent agent = Agent.Parse(strAgent);

                var profile = agentProfileService.GetAgentProfile(agent, profileId);
                if (profile == null)
                    return NotFound();

                var document = profile.Document;
                if (document == null)
                    return NotFound();

                string lastModified = document.LastModified.ToString("o");

                Response.ContentType = document.ContentType;
                Response.Headers.Add("LastModified", lastModified);
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                if (HttpMethods.IsHead(Request.Method))
                {
                    return NoContent();
                }

                return new FileContentResult(document.Content, document.ContentType);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[AcceptVerbs("GET", "HEAD")]
        [HttpGet(Order = 2)]
        [Produces("application/json")]
        public ActionResult GetAgentProfiles([FromQuery(Name = "agent")]string strAgent, DateTimeOffset? since = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(strAgent))
                {
                    throw new ArgumentNullException("agent");
                }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Agent agent = Agent.Parse(strAgent);

                var documents = agentProfileService.GetProfiles(agent, since);
                if (documents == null)
                    return Ok(new Guid[] { });

                IEnumerable<Guid> ids = documents.Select(x => x.Id);
                // TODO: Check for null
                string lastModified = documents.OrderByDescending(x => x.LastModified).FirstOrDefault().LastModified.ToString("o");

                Response.Headers.Add("LastModified", lastModified);
                return Ok(ids);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
        public ActionResult SaveAgentProfile(string profileId, [FromQuery(Name = "agent")]string strAgent, [FromBody]byte[] content)
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

                Response.Headers.Add("ETag", $"\"{document.Checksum}\"");
                Response.Headers.Add("LastModified", document.LastModified.ToString("o"));
                return NoContent();
            }
            catch (Exception ex)
            {
                // TODO: If exception is by ETagMatchException
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        public ActionResult DeleteProfile(string profileId, [FromQuery(Name = "agent")]string strAgent)
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

                // TODO: Concurrency

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
