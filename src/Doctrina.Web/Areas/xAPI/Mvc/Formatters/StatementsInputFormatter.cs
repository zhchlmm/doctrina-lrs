using Doctrina.xAPI;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Web.Areas.xAPI.Mvc.Formatters
{
    public class StatementsInputFormatter : TextInputFormatter
    {
        public StatementsInputFormatter()
        {
            // This formatter supports JSON.
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
            SupportedEncodings.Add(Encoding.UTF8);
        }

        protected override bool CanReadType(Type type)
        {
            // Any Enumerable type of TestModel (Lists, arrays) can be read by this formatter.
            return typeof(IEnumerable<Statement>).IsAssignableFrom(type);
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            ILogger logger = context.HttpContext.RequestServices.GetService(typeof(ILogger<StatementsInputFormatter>)) as ILogger;

            var request = context.HttpContext.Request;
            using (var reader = context.ReaderFactory(request.Body, encoding))
            using (var jsonReader = new JsonTextReader(reader))
            {
                // Important: Don't close the request stream because you many need it in other middleware or controllers.
                jsonReader.CloseInput = false;

                bool isSuccessful = true;
                object model = null;
                try
                {
                    // Load the body and determine if it's an array or single object.
                    JToken token = JToken.Load(jsonReader);
                    if (token.Type == JTokenType.Array)
                    {
                        model = token.ToObject<IEnumerable<Statement>>();
                    }
                    else if(token.Type == JTokenType.Object)
                    {
                        Statement single = token.ToObject<Statement>();
                        model = new Statement[] { single };
                    }
                }
                catch (Exception ex)
                {
                    // Serialization failed, probably because it wasn't correct JSON.
                    logger.LogError(0, ex, "Error parsing Statement.");
                    isSuccessful = false;
                }

                // Return a InputFormatterResult based off of deserialization status.
                // There are non-async methods to if your input formatter needs to await anything.
                if (isSuccessful)
                {
                    if (model == null && !context.TreatEmptyInputAsDefaultValue)
                    {
                        return InputFormatterResult.NoValueAsync();
                    }
                    else
                    {
                        return InputFormatterResult.SuccessAsync(model);
                    }
                }

                return InputFormatterResult.FailureAsync();
            }
        }
    }
}
