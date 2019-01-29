using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Doctrina.xAPI.LRS.Controllers
{
    [Area("xapi")]
    [Route("xapi/about")]
    [Produces("application/json")]
    public class AboutController : ApiControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<About> About()
        {
            var about = new About()
            {
                Version = ApiVersion.GetSupported().Select(x => x.Key)
            };
            
            return Ok(about);
        }
    }
}
