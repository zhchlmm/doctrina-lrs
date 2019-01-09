using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Doctrina.Web.Areas.xAPI.Routing
{
    public class AlternateRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] allowedMethodNames = new string[] { "POST", "GET", "PUT", "DELETE" };
        private readonly string[] formHttpHeaders = new string[] { "Authorization", "X-Experience-API-Version", "Content-Type", "Content-Length", "If-Match", "If-None-Match" };
        // TODO: This might work in most chases but is not really valid.
        private readonly Regex unsafeUrlRegex =new Regex(@"^-\]_.~!*'();:@&=+$,/?%#[A-z0-9]");

        public AlternateRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var request = context.Request;
                // All requests issued must be post
                if (request.Method.ToUpperInvariant() != "POST")
                {
                    await _next(context);
                    return;
                }

                // Multiple query parameters are not allowed
                if (context.Request.Query.Count != 1)
                {
                    await _next(context);
                    return;
                }

                // Must include parameter method
                var methodQuery = context.Request.Query["method"].FirstOrDefault();
                if (string.IsNullOrWhiteSpace(methodQuery))
                {
                    await _next(context);
                    return;
                }

                if (!allowedMethodNames.Contains(methodQuery.ToUpperInvariant()))
                {
                    await _next(context);
                    return;
                }
                // Change currect request method
                request.Method = methodQuery;

                // Parse form data values
                var formData = request.Form.ToDictionary(x => x.Key, y => y.Value.ToString());

                if (new string[] { "POST", "PUT" }.Contains(methodQuery))
                {
                    if (!formData.ContainsKey("content"))
                    {
                        // An LRS will reject an alternate request syntax sending content which does not have a form parameter with the name of \"content\" (Communication 1.3.s3.b4)
                        context.Response.StatusCode = 400;
                        throw new Exception("Alternate request syntax sending content does not have a form parameter with the name of \"content\"");
                    }

                    // Content-Type form header is not required
                    //if (!formData.Any(x=> x.Key.Equals("Content-Type", StringComparison.InvariantCultureIgnoreCase)))
                    //{
                    //    context.Response.StatusCode = 400;
                    //    throw new Exception("Alternate request syntax sending content does not have a form header parameter with the name of \"Content-Type\"");
                    //}
                }

                if (formData.ContainsKey("content"))
                {
                    string urlEncodedContent = formData["content"];

                    if (unsafeUrlRegex.IsMatch(urlEncodedContent))
                    {
                        throw new Exception($"Form data 'content' contains unsafe charactors.");
                    }

                    string decodedContent = HttpUtility.UrlDecode(urlEncodedContent);

                    var requestContent = new StringContent(decodedContent);
                    request.Body = await requestContent.ReadAsStreamAsync();
                    formData.Remove("content");
                }

                // Treat all known form headers as request http headers
                if (formData.Any())
                {
                    foreach (var headerName in formHttpHeaders)
                    {
                        var formHeader = formData.FirstOrDefault(x => x.Key.Equals(headerName, StringComparison.InvariantCultureIgnoreCase));
                        if (!formHeader.Equals(default(KeyValuePair<string, string>)))
                        {
                            request.Headers[formHeader.Key] = formHeader.Value;
                            formData.Remove(formHeader.Key);
                        }
                    }
                }

                // Treat the rest as query parameters
                if (formData.Any())
                {
                    var queryCollection = HttpUtility.ParseQueryString(string.Empty);
                    foreach (var name in formData)
                    {
                        queryCollection.Add(name.Key, name.Value);
                    }
                    if (queryCollection.Count > 0)
                    {
                        request.QueryString = new QueryString("?" + queryCollection.ToString());
                    }
                    else
                    {
                        request.QueryString = new QueryString();
                    }
                }

                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = 400;
                    using (var writer = new StreamWriter(context.Response.Body))
                    {
                        await writer.WriteAsync(ex.Message);
                    }
                }
            }
        }
    }

    public static class AlternateRequestMiddlewareExtensions
    {
        public static IApplicationBuilder UseAlternateRequestSyntax(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AlternateRequestMiddleware>();
        }
    }
}
