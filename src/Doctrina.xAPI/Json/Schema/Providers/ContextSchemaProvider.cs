using Doctrina.xAPI.Models;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Json.Schema.Providers
{
    public class ContextSchemaProvider : JSchemaGenerationProvider
    {
        public override JSchema GetSchema(JSchemaTypeGenerationContext context)
        {
            var agentSchema = context.Generator.Generate(typeof(Agent));
            var groupSchema = context.Generator.Generate(typeof(Group));
            var contextActivitiesSchema = context.Generator.Generate(typeof(ContextActivities));
            var statementRefSchema = context.Generator.Generate(typeof(StatementRef));
            var extensionsSchema = context.Generator.Generate(typeof(Models.Extensions));

            var contextSchema = new JSchema()
            {
                Id = new Uri("Context", UriKind.Relative ),
                Type = JSchemaType.Object,
                Properties =
                {
                    { "registration", new JSchema(){ Type = JSchemaType.String } },
                    { "instructor", new JSchema(){ OneOf = { agentSchema, groupSchema } } },
                    { "team", new JSchema(){ OneOf = { groupSchema } } },
                    { "contextActivities", contextActivitiesSchema },
                    { "revision", new JSchema(){ Type = JSchemaType.String } },
                    { "platform", new JSchema(){ Type = JSchemaType.String } },
                    { "language", new JSchema(){ Type = JSchemaType.String } },
                    { "statement", statementRefSchema },
                    { "extensions", extensionsSchema }
                }
            };

            return contextSchema;
        }
    }
}
