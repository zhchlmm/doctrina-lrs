using Doctrina.xAPI.Client.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Doctrina.xAPI.Store.Controllers
{
    [ApiController]
    [Route("xapi/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        public Agent Authority
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

        public ApiVersion APIVersion
        {
            get
            {
                return Request.Headers[Headers.XExperienceApiVersion].ToString();
            }
        }
    }
}
