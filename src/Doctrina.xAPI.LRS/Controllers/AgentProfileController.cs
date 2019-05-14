using Doctrina.Persistence.Services;
using Doctrina.xAPI.LRS.Mvc.Filters;
using Doctrina.xAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using Doctrina.Application.AgentProfiles.Commands;
using System.Threading.Tasks;
using Doctrina.Application.AgentProfiles.Queries;

namespace Doctrina.xAPI.LRS.Controllers
{
    [HeadWithoutBody]
    [RequiredVersionHeader]
    [Route("xapi/agents/profile")]
    [Produces("application/json")]
    public class AgentProfileController : ApiControllerBase
    {
        private IMediator _mediator;

        public AgentProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AcceptVerbs("GET", "HEAD", Order = 1)]
        public async Task<ActionResult> GetAgentProfile(string profileId, [FromQuery(Name = "agent")]string strAgent)
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

                var profile = await _mediator.Send(new GetAgentProfileQuery()
                {
                    ProfileId = profileId,
                    Agent = agent
                });

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
        public async Task<ActionResult> GetAgentProfilesAsync([FromQuery(Name = "agent")]string strAgent, DateTimeOffset? since = null)
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

                var profiles = await _mediator.Send(new GetAgentProfilesQuery(agent, since));

                if (profiles == null)
                    return Ok(new Guid[] { });

                // TODO: Move this logic into a response model from command
                IEnumerable<Guid> ids = profiles.Select(x => x.AgentProfileId).ToList();
                // TODO: Check for null
                string lastModified = profiles.OrderByDescending(x => x.Document.LastModified).FirstOrDefault()?.Document.LastModified.ToString("o");

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
        public async Task<ActionResult> SaveAgentProfileAsync(string profileId, [FromQuery(Name = "agent")]string strAgent, [FromBody]byte[] content)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                string contentType = Request.ContentType;
                Agent agent = Agent.Parse(strAgent);

                var profile = await _mediator.Send(new MergeAgentProfileCommand()
                {
                    Agent = agent,
                    ProfileId = profileId,
                    Content = content,
                    ContentType = contentType
                });

                var document = profile.Document;

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
        public async Task<ActionResult> DeleteProfileAsync(string profileId, [FromQuery(Name = "agent")]string strAgent)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                string contentType = Request.ContentType;
                Agent agent = Agent.Parse(strAgent);

                var profile = await _mediator.Send(GetAgentProfileQuery.Create(agent, profileId));
                if (profile == null)
                    return NotFound();

                // TODO: Concurrency

                await _mediator.Send(new DeleteAgentProfileCommand() {
                    ProfileId = profileId,
                    Agent = agent
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
