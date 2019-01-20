using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Doctrina.xAPI;
using Microsoft.AspNetCore.Authorization;

namespace Doctrina.Web.Areas.xAPI.Controllers
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
            
            return new JsonResult(about);
        }
    }
}
