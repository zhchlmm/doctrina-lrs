using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Doctrina.xAPI;
using Doctrina.xAPI.Models;
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
                Version = XAPIVersion.GetSupported().Select(x => x.Key)
            };
            
            return new JsonResult(about);
        }
    }
}
