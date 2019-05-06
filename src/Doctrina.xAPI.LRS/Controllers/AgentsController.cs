using Doctrina.Persistence.Services;
using Doctrina.xAPI.LRS.Mvc.Filters;
using Doctrina.xAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Doctrina.xAPI.LRS.Controllers
{
    [HeadWithoutBody]
    [RequiredVersionHeaderAttribute]
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
