using Doctrina.xAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;

namespace Doctrina.Web.Mvc.ModelBinders
{
    public class StatementModelBinder : IModelBinder
    {
        private static JSchema _schema;
        private JSchema Schema
        {
            get
            {
                if(_schema == null)
                {
                    //JSchemaGenerator generator  = new JSchemaGenerator();
                    //generator.GenerationProviders.Add(new StringEnumGenerationProvider());
                    //JSchema schema = generator .Generate(typeof(Statement));
                    ////_schema = JSchema.From.FromTypeAsync<Statement>().Result;
                    //schema.AllowAdditionalProperties = false;
                    //_schema = schema;

                    using (System.IO.StreamReader file = System.IO.File.OpenText(@"result.schema.json"))
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        _schema = JSchema.Load(reader);
                    }
                }
                return _schema;
            }
        }


        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            // Specify a default argument name if none is set by ModelBinderAttribute
            var modelName = bindingContext.BinderModelName;
            if (string.IsNullOrEmpty(modelName))
            {
                modelName = "statements";
            }

            if (bindingContext.ModelType != typeof(Statement))
                return Task.CompletedTask;
            var request = bindingContext.ActionContext.HttpContext.Request;

            string json = null;
            using(var streamReader = new System.Net.Http.StreamContent(request.Body))
            {
                json = streamReader.ReadAsStringAsync().Result;
            }

            JsonTextReader jsonReader = new JsonTextReader(new System.IO.StringReader(json));

            JSchemaValidatingReader validatingReader = new JSchemaValidatingReader(jsonReader);
            validatingReader.Schema = Schema;
            validatingReader.ValidationEventHandler += delegate (object sender, SchemaValidationEventArgs args) {
                bindingContext.ModelState.AddModelError(args.Path, args.Message);
            };

            JsonSerializer serializer = new JsonSerializer();
            serializer.CheckAdditionalContent = true;
            //serializer.Converters.Add(new AgentConverter());
            //serializer.Converters.Add(new StatementTargetConverter());
            serializer.Error += delegate (object sender, ErrorEventArgs args)
            {
                bindingContext.ModelState.AddModelError(args.ErrorContext.Path, args.ErrorContext.Error.Message);
                args.ErrorContext.Handled = true;
            };

            var statement = serializer.Deserialize<Statement>(validatingReader);
            bindingContext.Result = ModelBindingResult.Success(new Statement[] { statement });

            return Task.CompletedTask;
        }
    }
}
