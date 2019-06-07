using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Store.Routing
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private const int ReadChunkBufferLength = 4096;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory
                .CreateLogger<RequestResponseLoggingMiddleware>();
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.HasValue && context.Request.Path.Value.StartsWith("/xapi/"))
            {
                if (context == null) throw new ArgumentNullException(nameof(context));

                var sw = Stopwatch.StartNew();
                await LogRequestAsync(context.Request);
                await _next.Invoke(context);
                await LogResponseAsync(context, sw);
            }
            else
            {
                await _next.Invoke(context);
            }
        }

        private async Task LogRequestAsync(HttpRequest request)
        {
            request.EnableRewind();

            _logger.LogInformation($"{request.Method} " +
                                   $"{request.Scheme}://" +
                                   $"{request.Host}" +
                                   $"{request.Path}" +
                                   $"{request.QueryString}");
            //using (var requestStream = _recyclableMemoryStreamManager.GetStream())
            //{
            //    request.Body.CopyTo(requestStream);
            //    _logger.LogDebug($"Request Body: { ReadStreamInChunks(requestStream)}");
            //}
            await Task.FromResult(0);
        }

        private async Task LogResponseAsync(HttpContext context, Stopwatch sw)
        {
            var statusCode = context.Response?.StatusCode;

            sw.Stop();
            var elapsed = sw.Elapsed.TotalMilliseconds;

            string message = $"{context.Request.Method} " +
                                      $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path} " +
                                      $"responded {statusCode} in {elapsed:0.0000} ms";
            if (sw.ElapsedMilliseconds > 499)
            {
                _logger.LogWarning(message);
            }
            else
            {
                _logger.LogInformation(message);
            }

#if DEBUG
            //var originalBody = context.Response.Body;
            //using (var responseStream = _recyclableMemoryStreamManager.GetStream())
            //{
            //    context.Response.Body = responseStream;
            //    await responseStream.CopyToAsync(originalBody);
            //    _logger.LogDebug($"Response Body: { ReadStreamInChunks(responseStream)}");
            //}
            //context.Response.Body = originalBody;
#endif
            await Task.FromResult(0);
        }

        //private static string ReadStreamInChunks(Stream stream)
        //{
        //    stream.Seek(0, SeekOrigin.Begin);
        //    string result;
        //    using (var textWriter = new StringWriter())
        //    using (var reader = new StreamReader(stream))
        //    {
        //        var readChunk = new char[ReadChunkBufferLength];
        //        int readChunkLength;
        //        //do while: is useful for the last iteration in case readChunkLength < chunkLength
        //        do
        //        {
        //            readChunkLength = reader.ReadBlock(readChunk, 0, ReadChunkBufferLength);
        //            textWriter.Write(readChunk, 0, readChunkLength);
        //        } while (readChunkLength > 0);

        //        result = textWriter.ToString();
        //    }

        //    return result;
        //}
    }
}
