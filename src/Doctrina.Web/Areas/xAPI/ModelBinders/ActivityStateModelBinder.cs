using Doctrina.Web.Areas.xAPI.Models;
using Doctrina.xAPI;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Doctrina.Web.Areas.xAPI.Mvc.ModelBinders
{
    public class ActivityStateModelBinder : IModelBinder
    {
        static ActivityStateModelBinder()
        {

        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            // Specify a default argument name if none is set by ModelBinderAttribute
            //var modelName = bindingContext.BinderModelName;
            //if (string.IsNullOrEmpty(modelName))
            //{
            //    modelName = "document";
            //}

            //var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            //if (valueProviderResult == ValueProviderResult.None)
            //{
            //    return Task.CompletedTask;
            //}

            var model = new StateDocumentModel();
            var actionContext = bindingContext.ActionContext;
            var httpContext = actionContext.HttpContext;
            var request = httpContext.Request;
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

            // Get agent from uri
            if (request.Query.ContainsKey("agent"))
            {
                string strAgent = request.Query["agent"];
                model.Agent = ParseAgent(strAgent, bindingContext.ModelState);
            }

            if (request.Query.ContainsKey("activityId"))
            {
                string strActivityId = request.Query["activityId"];
                if (Iri.TryParse(strActivityId, out Iri activityId))
                {
                    model.ActivityId = activityId;
                }
                else
                {
                    bindingContext.ModelState.AddModelError("activityId", "Not a valid uri");
                }
            }

            if (request.Query.ContainsKey("stateId"))
            {
                string strStateId = request.Query["stateId"];
                model.StateId = strStateId;
            }

            if (request.Query.ContainsKey("registration"))
            {
                string strRegistration = request.Query["registration"];
                if (Guid.TryParse(strRegistration, out Guid registration))
                {
                    model.Registration = registration;
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

        public Agent ParseAgent(string jsonString, ModelStateDictionary modelState)
        {
            try
            {
                var agent = JsonConvert.DeserializeObject<Agent>(jsonString);
                return agent;
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
