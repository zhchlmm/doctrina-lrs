using Doctrina.xAPI;
using Doctrina.xAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

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
            using (var streamReader = new System.Net.Http.StreamContent(request.Body))
            {
                json = streamReader.ReadAsStringAsync().Result;
            }

            Statement statement = DeserializeStatement(bindingContext, json);

            if (statement != null)
            {
                bindingContext.Result = ModelBindingResult.Success(statement);
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;
        }

        private Statement DeserializeStatement(ModelBindingContext bindingContext, string json)
        {
            var request = bindingContext.ActionContext.HttpContext.Request;

            JsonTextReader jsonReader = new JsonTextReader(new System.IO.StringReader(json));

            JSchemaValidatingReader validatingReader = new JSchemaValidatingReader(jsonReader)
            {
                Schema = Schema
            };
            validatingReader.ValidationEventHandler += delegate (object sender, SchemaValidationEventArgs args)
            {
                bindingContext.ModelState.AddModelError(args.Path, args.Message);
            };

            string strVersion = request.Headers[Constants.Headers.XExperienceApiVersion];
            if (string.IsNullOrWhiteSpace(strVersion))
            {
                throw new Exception($"'{Constants.Headers.XExperienceApiVersion}' is missing.");
            }

            XAPISerializer serializer = new XAPISerializer(strVersion);
            serializer.Error += delegate (object sender, ErrorEventArgs args)
            {
                bindingContext.ModelState.AddModelError(args.ErrorContext.Path, args.ErrorContext.Error.Message);
                args.ErrorContext.Handled = true;
            };

            var statement = serializer.Deserialize<Statement>(validatingReader);
            return statement;
        }

    }
}
