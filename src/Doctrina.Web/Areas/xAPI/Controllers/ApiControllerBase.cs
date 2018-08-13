using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Doctrina.xAPI.Models;
using Doctrina.xAPI;

namespace Doctrina.Web.Areas.xAPI.Controllers
{
    [ApiController]
    [Route("xapi/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        public Agent CurrentAuthority
        {
            get
            {
                var uriBuilder = new UriBuilder()
                {
                    Host = Request.Host.Value,
                    Scheme = Request.Scheme,
                };

                return new Agent()
                {
                    Name = User.Identity.Name,
                    OpenId = new Iri($"{Request.Scheme}://{Request.Host.Value}")
                };
            }
        }

        public XAPIVersion APIVersion
        {
            get
            {
                return Request.Headers[Constants.Headers.XExperienceApiVersion].ToString();
            }
        }
    }
}
