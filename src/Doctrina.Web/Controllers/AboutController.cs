using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Doctrina.xAPI;
using Doctrina.xAPI.Models;

namespace Doctrina.Web.Controllers
{
    [Route("xapi/about")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        [HttpGet(Name = "GetAbout")]
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
