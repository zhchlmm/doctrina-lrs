using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Json.Schema.Providers
{
    public class StatementBaseSchemaProvider : JSchemaGenerationProvider
    {
        public override JSchema GetSchema(JSchemaTypeGenerationContext context)
        {
            var statementBaseSchema = new JSchema()
            {
                AllowAdditionalProperties = false,
                Required = { "actor", "verb", "object" },
                OneOf = {
                    new JSchema(){ Required = {"object" },
                        Properties = {
                            { "object", new JSchema() { Reference = new Uri("#/definitions/activity") } }
                        }
                    }
                },
                Properties =
                {

                    { "actor", new JSchema() },
                    { "verb", new JSchema() }
                }
            };

            return statementBaseSchema;
        }
    }
}
