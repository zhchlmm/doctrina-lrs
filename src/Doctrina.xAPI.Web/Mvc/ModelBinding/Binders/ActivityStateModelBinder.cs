using Doctrina.xAPI.LRS.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Doctrina.xAPI.LRS.Mvc.ModelBinding
{
    public class ActivityStateModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var model = new StateDocumentModel();
            var request = bindingContext.ActionContext.HttpContext.Request;

            if (request.Method == HttpMethod.Post.Method || request.Method == HttpMethod.Put.Method)
            {
                // Parse contentType
                var contentType = MediaTypeHeaderValue.Parse(request.ContentType);
                model.ContentType = contentType.MediaType;
                // Validate content as valid json if application/json

                using(var reader = new StreamContent(request.Body))
                {
                    var binaryDocument = reader.ReadAsByteArrayAsync().Result;
                    model.Content = binaryDocument;
                }

                if(contentType.MediaType == MediaTypes.Application.Json)
                {
                    string jsonString = System.Text.Encoding.UTF8.GetString(model.Content);
                    ValidateJson(jsonString, bindingContext.ModelState);
                }
            }

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }

        public string ValidateJson(string jsonString, ModelStateDictionary modelState)
        {
            try
            {
                var obj = JToken.Parse(jsonString);
                return jsonString;
            }
            catch (JsonReaderException jex)
            {
                //Exception in parsing json
                Console.WriteLine(jex.Message);
                modelState.AddModelError(jex.Path, jex.Message);
            }
            catch (Exception ex) //some other exception
            {
                modelState.AddModelError(ex.Source, ex.Message);
            }

            return null;
        }
    }
}
