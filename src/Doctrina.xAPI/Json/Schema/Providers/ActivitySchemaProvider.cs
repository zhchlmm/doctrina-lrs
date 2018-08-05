using Doctrina.xAPI.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Schema.Providers
{
    public class ActivitySchemaProvider : JSchemaGenerationProvider
    {
        public override JSchema GetSchema(JSchemaTypeGenerationContext context)
        {
            var activityDefinitionSchema = context.Generator.Generate(typeof(ActivityDefinition));

            var activitySchema = new JSchema()
            {
                Type = JSchemaType.Object,
                Properties =
                {
                    { "id", new JSchema(){ Type = JSchemaType.String, Format = "uri" } },
                    { "definition", activityDefinitionSchema },
                    { "objectType", new JSchema(){ Type = JSchemaType.String, Enum = { "Activity" } } }
                },
                Required = { "id" }
                
            };

            return activitySchema;
        }
    }
}
