﻿using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Store.Routing
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
            if (context.Request.Path.HasValue && context.Request.Path.Value.StartsWith("/xapi/"))
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
            }

            await _next.Invoke(context);
        }
    }
}
