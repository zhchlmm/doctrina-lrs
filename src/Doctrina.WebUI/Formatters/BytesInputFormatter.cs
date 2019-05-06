using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.WebUI.Formatters
{
    public class BytesInputFormatter : TextInputFormatter
    {
        public BytesInputFormatter()
        {
            // This formatter supports JSON.
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/plain"));
            SupportedEncodings.Add(Encoding.UTF8);
        }

        public override bool CanRead(InputFormatterContext context)
        {
            return true;
        }

        protected override bool CanReadType(Type type)
        {
            return type == typeof(byte[]);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            var request = context.HttpContext.Request;
            using (var reader = new StreamReader(request.Body))
            {
                var content = await reader.ReadToEndAsync();
                var bytes = encoding.GetBytes(content);
                return await InputFormatterResult.SuccessAsync(bytes);
            }
        }
    }
}
