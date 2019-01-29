using Doctrina.Persistence.Services;
using Doctrina.xAPI.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Doctrina.xAPI.LRS.Routing
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
            // TODO: https://github.com/adlnet/xAPI-Spec/blob/master/xAPI-Communication.md#user-content-2.1.3.s2.b5
            if (!headers.ContainsKey(headerKey))
            {
                var date = statementService.GetConsistentThroughDate();
                if (!headers.ContainsKey(headerKey))
                {
                    headers.Add(headerKey, date.ToString("o"));
                }
            }

            // Execute next
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
