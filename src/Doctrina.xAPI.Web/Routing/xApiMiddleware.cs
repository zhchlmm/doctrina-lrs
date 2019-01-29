using Microsoft.AspNetCore.Builder;

namespace Doctrina.xAPI.LRS.Routing
{
    public static class xApiMiddlewareExtensions
    {
        public static IApplicationBuilder UseXAPI(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>()
                .UseAlternateRequestSyntax()
                .UseConsistentThrough()
                .UseUnrecognizedParameters();
        }
    }
}
