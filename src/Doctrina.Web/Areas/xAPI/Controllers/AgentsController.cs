using Doctrina.Core.Services;
using Doctrina.Web.Areas.xAPI.Mvc.Filters;
using Doctrina.xAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Doctrina.Web.Areas.xAPI.Controllers
{
    [HeadWithoutBody]
    [VersionHeader]
    [Route("xapi/agents")]
    [Produces("application/json")]
    public class AgentsController : ApiControllerBase
    {
        private IAgentService _agentService;

        public AgentsController(IAgentService agentService)
        {
            _agentService = agentService;
        }

        [AcceptVerbs("GET", "HEAD")]
        public ActionResult GetAgentProfile([FromQuery(Name = "agent")]string strAgent)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Agent agent = Agent.Parse(strAgent);

                var person = _agentService.GetPerson(agent);
                if (person == null)
                    return NotFound();

                if (HttpMethods.IsHead(Request.Method))
                {
                    return NoContent();
                }

                return Ok(person);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
