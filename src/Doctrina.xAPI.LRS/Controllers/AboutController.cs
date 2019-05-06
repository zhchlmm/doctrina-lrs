using Doctrina.Application.About.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Doctrina.xAPI.LRS.Controllers
{
    [Area("xapi")]
    [Route("xapi/about")]
    [Produces("application/json")]
    public class AboutController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public AboutController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<About>> About()
        {
            return Ok(await _mediator.Send(new GetAboutQuery()));
        }
    }
}
