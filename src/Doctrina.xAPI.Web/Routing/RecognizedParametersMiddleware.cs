using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.LRS.Routing
{
    public class UnrecognizedParametersMiddleware
    {
        private readonly string[] recognizedParameters = new string[]
        {
            "profileId",
            "statementId",
            "voidedStatementId",
            "agent",
            "verb",
            "activity",
            "registration",
            "related_activities",
            "related_agents",
            "since",
            "until",
            "limit",
            "format",
            "attachments",
            "ascending",
            "skip",
            "activityId",
            "stateId"
        };

        private readonly RequestDelegate _next;

        public UnrecognizedParametersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestParameters = context.Request.Query.Select(x => x.Key);

            foreach (var requestParameter in requestParameters)
            {
                if (string.IsNullOrWhiteSpace(requestParameter))
                    continue;

                if (!recognizedParameters.Contains(requestParameter))
                {
                    throw new Exception("Unrecognized parameter: " + requestParameter);
                }
            }

            await _next.Invoke(context);
        }
    }

    public static class UnrecognizedParametersMiddlewareExtensions
    {

        /// <summary>
        /// Bad Request to any request which uses a parameter with differing case
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseUnrecognizedParameters(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UnrecognizedParametersMiddleware>();
        }
    }
}
