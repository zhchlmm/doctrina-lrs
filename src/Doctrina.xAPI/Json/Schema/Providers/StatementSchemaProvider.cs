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
            var statementBaseSchema = context.Generator.Generate(typeof(StatementBase));
            var agentSchema = context.Generator.Generate(typeof(Agent));
            var groupSchema = context.Generator.Generate(typeof(Group));

            var statementSchema = new JSchema()
            {
                Id = new Uri("Statement", UriKind.Relative),
                AllowAdditionalProperties = false,
                AllOf =
                {
                    statementBaseSchema
                },
                Properties =
                {
                    { "id", new JSchema(){ Type = JSchemaType.String, Format = "uuid" } },
                    { "stored", new JSchema(){ Type = JSchemaType.String, Format = "date-time" } },
                    { "authority", new JSchema(){
                        Type = JSchemaType.Object,
                        OneOf =
                        {
                            agentSchema,
                            groupSchema
                        },
                        Properties =
                        {
                            { "objectType", new JSchema()
                                {
                                    Type = JSchemaType.String,
                                    Enum = { "Agent", "Group" }
                                }
                            }
                        }
                    }
                    },
                    { "version", new JSchema(){ Type = JSchemaType.String } }
                },
                ExtensionData =
                {
                    ["definitions"] = new JObject()
                    {
                        ["StatementBase"] = statementBaseSchema,
                        ["Agent"] = agentSchema
                    }
                }
            };

            return statementSchema;
        }
    }
}
