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

namespace UmbracoLRS.Core.ModelBinders
{
    public class StatementsModelBinder : IModelBinder
    {
        private static JSchema _schema;
        private JSchema Schema
        {
            get
            {
                if(_schema == null)
                {
                    JSchemaGenerator generator  = new JSchemaGenerator();
                    generator.GenerationProviders.Add(new StringEnumGenerationProvider());
                    JSchema schema = generator .Generate(typeof(Statement));
                    //_schema = JSchema.From.FromTypeAsync<Statement>().Result;
                    schema.AllowAdditionalProperties = false;
                    _schema = schema;
                }
                return _schema;
            }
        }

        static StatementsModelBinder()
        {

        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(Statement[]))
                return false;

            string json = actionContext.Request.Content.ReadAsStringAsync().Result;

            JsonTextReader reader = new JsonTextReader(new System.IO.StringReader(json));

            JSchemaValidatingReader validatingReader = new JSchemaValidatingReader(reader);
            validatingReader.Schema = Schema;
            validatingReader.ValidationEventHandler += delegate (object sender, SchemaValidationEventArgs args) {
                bindingContext.ModelState.AddModelError(args.Path, args.Message);
            };

            JsonSerializer serializer = new JsonSerializer();
            serializer.CheckAdditionalContent = true;
            serializer.Converters.Add(new AgentConverter());
            serializer.Converters.Add(new StatementTargetConverter());
            serializer.Error += delegate (object sender, ErrorEventArgs args)
            {
                bindingContext.ModelState.AddModelError(args.ErrorContext.Path, args.ErrorContext.Error.Message);
                args.ErrorContext.Handled = true;
            };

            if (json.StartsWith("["))
            {
                bindingContext.Model = serializer.Deserialize<Statement[]>(validatingReader);
            }
            else
            {
                var statement = serializer.Deserialize<Statement>(validatingReader);
                bindingContext.Model = new Statement[] { statement };
            }

            bindingContext.ValidationNode.Validate(actionContext);

            if (!bindingContext.ModelState.IsValid){
                return false;
            }
            //var statement = JsonConvert.DeserializeObject<Statement>(jstatement.ToString(), settings);

            return true;
        }
    }
}
