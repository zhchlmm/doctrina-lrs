using Doctrina.xAPI.Models;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Schema.Providers
{
    public class ContextActivitiesSchemaProvider : JSchemaGenerationProvider
    {
        public override JSchema GetSchema(JSchemaTypeGenerationContext context)
        {
            //var activitySchema = context.Generator.Generate(typeof(Activity));

            var oneOf = new JSchema()
            {
                Id = new Uri("MultipleOrSingleActivity", UriKind.Relative),
                AllowAdditionalProperties = false,
                OneOf =
                {
                    new JSchema()
                    {
                        Type = JSchemaType.Array,
                        Items = { new JSchema() { Reference = new Uri("#/definitions/Activity",UriKind.Relative) } }
                    },
                    new JSchema() { Reference = new Uri("#/definitions/Activity",UriKind.Relative) }
                }
            };

            var caSchema = new JSchema()
            {
                Id = new Uri("ContextActivities", UriKind.Relative),
                Type = JSchemaType.Object,
                AllowAdditionalProperties = false,
                Properties =
                {
                    { "parent", oneOf },
                    { "grouping", oneOf },
                    { "category", oneOf },
                    { "other", oneOf }
                }
            };

            //var schema = JSchema.Parse(schemaJson);
            return caSchema;
        }
    }
}
