using ExperienceAPI.Core.Models;
using ExperienceAPI.Core.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using UmbracoLRS.Core.Models;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace UmbracoLRS.Core.ModelBinders
{
    public class StateDocumentModelBinder : IModelBinder
    {
        static StateDocumentModelBinder()
        {

        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(StateDocumentModel))
                return false;

            try
            {
                var model = new StateDocumentModel();


                if (actionContext.Request.Method == HttpMethod.Post || actionContext.Request.Method == HttpMethod.Put)
                {
                    // Parse contentType
                    string contentType = actionContext.Request.Content.Headers.ContentType.ToString();
                    model.MediaType = contentType;
                    // Validate content as valid json if application/json
                    if (contentType == "application/json")
                    {
                        string strContent = actionContext.Request.Content.ReadAsStringAsync().Result;
                        model.JsonDocument = ValidateJson(strContent, bindingContext.ModelState);
                    }
                    else
                    {
                        byte[] binaryDocument = actionContext.Request.Content.ReadAsByteArrayAsync().Result;
                        model.BinaryDocument = binaryDocument;
                    }
                }

                // Get agent from uri
                var uriParameters = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);
                var strAgent = uriParameters.Get("agent");
                model.Agent = ParseAgent(strAgent, bindingContext.ModelState);

                string strActivityId = uriParameters.Get("activityId");
                if(Uri.TryCreate(strActivityId, UriKind.Absolute, out Uri activityId))
                {
                    model.ActivityId = activityId;
                }
                else
                {
                    bindingContext.ModelState.AddModelError("activityId", "Not a valid uri");
                }

                string strStateId = uriParameters.Get("stateId");
                model.StateId = strStateId;

                string strRegistration = uriParameters.Get("registration");
                if (Guid.TryParse(strActivityId, out Guid registration))
                {
                    model.Registration = registration;
                }
              

                bindingContext.Model = model;

                bindingContext.ValidationNode.Validate(actionContext);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //public void ValidateModel<T>(ModelStateDictionary modelState, string jsonString)
        //{
        //    JsonTextReader reader = new JsonTextReader(new System.IO.StringReader(jsonString));

        //    JSchemaValidatingReader validatingReader = new JSchemaValidatingReader(reader);
        //    //validatingReader.Schema = Schema;
        //    validatingReader.ValidationEventHandler += delegate (object sender, SchemaValidationEventArgs args) {
        //        modelState.AddModelError(args.Path, args.Message);
        //    };

        //    JsonSerializer serializer = new JsonSerializer();
        //    serializer.CheckAdditionalContent = true;
        //    serializer.Error += delegate (object sender, ErrorEventArgs args)
        //    {
        //        modelState.AddModelError(args.ErrorContext.Path, args.ErrorContext.Error.Message);
        //        args.ErrorContext.Handled = true;
        //    };
        //}

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
