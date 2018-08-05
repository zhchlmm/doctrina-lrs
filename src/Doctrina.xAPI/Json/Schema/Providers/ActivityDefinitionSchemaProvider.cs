using Doctrina.xAPI.Models;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;

namespace Doctrina.xAPI.Json.Schema.Providers
{
    public class ActivityDefinitionSchemaProvider : JSchemaGenerationProvider
    {
        public override bool CanGenerateSchema(JSchemaTypeGenerationContext context)
        {
            return context.ObjectType == typeof(ActivityDefinition);
        }

        public override JSchema GetSchema(JSchemaTypeGenerationContext context)
        {
            var languageMapSchema = context.Generator.Generate(typeof(LanguageMap));
            var extensionSchema = context.Generator.Generate(typeof(Models.Extensions));

            var activityDefinitionSchema = new JSchema()
            {
                Type = JSchemaType.Object,
                Properties =
                {
                    { "name", languageMapSchema },
                    { "description", languageMapSchema },
                    { "type", new JSchema(){ Type = JSchemaType.String, Format = "uri"} },
                    { "moreInfo", new JSchema(){ Type = JSchemaType.String, Format = "uri" } },
                    { "extensions", extensionSchema }
                }
            };

            return activityDefinitionSchema;
        }
    }
}
