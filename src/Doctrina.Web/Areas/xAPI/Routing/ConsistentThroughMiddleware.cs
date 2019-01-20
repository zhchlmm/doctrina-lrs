using Doctrina.Core.Services;
using Doctrina.xAPI.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Doctrina.Web.Areas.xAPI.Routing
{
    public class ConsistentThroughMiddleware
    {
        private readonly RequestDelegate _next;

        public ConsistentThroughMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IStatementService statementService)
        {
            string headerKey = Headers.XExperienceApiConsistentThrough;
            var headers = context.Response.Headers;
            if (!headers.ContainsKey(headerKey))
            {
                var date = statementService.GetConsistentThroughDate();
                headers.Add(headerKey, date.ToString("o"));
            }
            await _next.Invoke(context);
        }
    }

    public static class ConsistentThroughMiddlewareExtensions
    {
        public static IApplicationBuilder UseConsistentThrough(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ConsistentThroughMiddleware>();
        }
    }
}
