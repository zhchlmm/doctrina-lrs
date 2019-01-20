using Doctrina.xAPI;
using Doctrina.xAPI.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Doctrina.Web.Areas.xAPI.Mvc.Filters
{
    /// <summary>
    /// Ensures the request has a supported 'X-Experience-Api-Version' header.
    /// </summary>
    public class VersionHeaderAttribute : Attribute, IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var supported = ApiVersion.GetSupported();
            try
            {
                if (!context.HttpContext.Request.Headers.ContainsKey(Headers.XExperienceApiVersion))
                    throw new Exception("Missing 'X-Experience-API-Version' header.");

                string requestVersion = context.HttpContext.Request.Headers[Headers.XExperienceApiVersion];
                if (string.IsNullOrEmpty(requestVersion))
                    throw new Exception("'X-Experience-API-Version' header or it's null or empty.");

                try
                {
                    ApiVersion version = (ApiVersion)requestVersion;
                    await next();
                }
                catch (Exception)
                {
                    throw new Exception($"'X-Experience-API-Version' header is '{requestVersion}' which is not supported.");
                }
            }
            catch (Exception ex)
            {
                context.HttpContext.Response.Headers.Add(Headers.XExperienceApiVersion, ApiVersion.GetLatest().ToString());
                context.Result = new BadRequestObjectResult(ex.Message + " Supported Versions are: " + string.Join(", ", supported.Select(x => x.Key)));
            }
        }
    }
}
