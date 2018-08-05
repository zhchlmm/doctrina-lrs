using Doctrina.xAPI.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Json.Schema.Providers
{
    public class StatementSchemaProvider : JSchemaGenerationProvider
    {
        public override JSchema GetSchema(JSchemaTypeGenerationContext context)
        {
            var agentSchema = context.Generator.Generate(typeof(Agent));

            var statementSchema = new JSchema()
            {
                Id = new Uri("Statement", UriKind.Relative),
                AllowAdditionalProperties = false,
                Properties =
                {
                    { "id", new JSchema(){ Type = JSchemaType.String, Format = "uuid" } },
                    { "actor", agentSchema }
                },
                ExtensionData =
                {
                    ["definitions"] = new JObject()
                    {
                        ["Agent"] = agentSchema
                    }
                }
            };

            return statementSchema;
        }
    }
}
