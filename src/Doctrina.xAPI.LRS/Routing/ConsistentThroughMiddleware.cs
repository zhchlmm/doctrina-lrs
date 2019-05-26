using Doctrina.Application.Statements.Queries;
using Doctrina.xAPI.Http;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
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

        public async Task InvokeAsync(HttpContext context, IMediator mediator)
        {
            if (context.Request.Path.HasValue && context.Request.Path.Value.StartsWith("/xapi/"))
            {
                string headerKey = Headers.XExperienceApiConsistentThrough;
                var headers = context.Response.Headers;
                // TODO: https://github.com/adlnet/xAPI-Spec/blob/master/xAPI-Communication.md#user-content-2.1.3.s2.b5
                if (!headers.ContainsKey(headerKey))
                {
                    DateTimeOffset? date = await mediator.Send(new GetConsistentThroughQuery());
                    if (!headers.ContainsKey(headerKey))
                    {
                        headers.Add(headerKey, date?.ToString("o"));
                    }
                }
            }

            // Execute next
            await _next.Invoke(context);
        }
    }
}
