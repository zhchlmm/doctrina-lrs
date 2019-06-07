using Doctrina.Application.Activities.Queries;
using Doctrina.xAPI.Store.Mvc.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Store.Controllers
{
    [HeadWithoutBody]
    [RequiredVersionHeader]
    [Route("xapi/activities")]
    [Produces("application/json")]
    public class ActivitiesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ActivitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AcceptVerbs("GET", "HEAD")]
        public async Task<ActionResult> GetActivityDocumentAsync(GetActivityQuery command)
        {
            Activity activity = await _mediator.Send(command);

            if (activity == null)
                return Ok(new Activity());

            // TODO: Return only canonical that match accept-language header, or und

            return Ok(activity);
        }
    }
}
