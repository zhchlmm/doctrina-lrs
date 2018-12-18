using Doctrina.xAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Doctrina.Web.Areas.xAPI.Mvc.Filters
{
    /// <summary>
    /// Accepts HEAD requests without body
    /// </summary>
    public class HeadWithoutBody : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.HttpContext.Request.Method == "HEAD")
            {
                context.Result = new StatusCodeResult(context.HttpContext.Response.StatusCode);
            }
        }
    }
}
