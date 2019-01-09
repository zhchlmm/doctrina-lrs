using Doctrina.Core;
using Doctrina.Core.Services;
using Doctrina.xAPI;
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
            //_statementService = statementService;
        }

        public async Task InvokeAsync(HttpContext context, IStatementService statementService)
        {
            var date = statementService.GetConsistentThroughDate();
            context.Response.Headers.Add("X-Experience-API-Consistent-Through", date.ToString("o"));
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
