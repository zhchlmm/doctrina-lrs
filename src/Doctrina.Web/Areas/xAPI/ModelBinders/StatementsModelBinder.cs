using Doctrina.Web.Areas.xAPI.Models;
using Doctrina.xAPI;
using Doctrina.xAPI.Http;
using Doctrina.xAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;

namespace Doctrina.Web.Areas.xAPI.Mvc.ModelBinders
{
    public class StatementsPostContentModelBinder : IModelBinder
    {
        private static JSchema _schema;

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                if (bindingContext.ModelType != typeof(StatementsPostContent))
                    return;

                var model = new StatementsPostContent();

                var request = bindingContext.ActionContext.HttpContext.Request;

                var version = request.Headers[Constants.Headers.XExperienceApiVersion];
                var content = new StatementsHttpContent(request.ContentType, request.Body);
                var jsonString = await content.ReadStatementsString();

                var serializer = new XAPISerializer((string)version);
                serializer.Error += delegate (object sender, ErrorEventArgs args)
                {
                    bindingContext.ModelState.AddModelError(args.ErrorContext.Path, args.ErrorContext.Error.Message);
                    args.ErrorContext.Handled = true;
                };
                var validationReader = CreateValidationReader(jsonString);

                validationReader.ValidationEventHandler += delegate (object sender, SchemaValidationEventArgs args)
                {
                    bindingContext.ModelState.AddModelError(args.Path, args.Message);
                };

                if(content.Attachments != null)
                {
                    // TODO: Attachments
                    //model.Attachments = content.Attachments;
                    throw new NotImplementedException();
                }

                if (jsonString.StartsWith("{"))
                {
                    var stmt = serializer.Deserialize<Statement>(validationReader);
                    model.Statements = new Statement[] { stmt };
                    
                }
                else if (jsonString.StartsWith("["))
                {
                    var stmts = serializer.Deserialize<Statement[]>(validationReader);
                    model.Statements = stmts;
                }
                else
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                    return;
                }

                bindingContext.Result = ModelBindingResult.Success(model);
                return;
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError("", ex, bindingContext.ModelMetadata);
                bindingContext.Result = ModelBindingResult.Failed();
            }
        }

        private JSchemaValidatingReader CreateValidationReader(string json)
        {
            JsonTextReader jsonReader = new JsonTextReader(new System.IO.StringReader(json));
            var validatingReader = new JSchemaValidatingReader(jsonReader);
            validatingReader.Schema = GetSchema();
            return validatingReader;
        }

        private JSchema GetSchema()
        {
            if (_schema == null)
            {
                using (System.IO.StreamReader file = System.IO.File.OpenText(@"result.schema.json"))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    _schema = JSchema.Load(reader);
                }

            }
            return _schema;
        }
    }
}
