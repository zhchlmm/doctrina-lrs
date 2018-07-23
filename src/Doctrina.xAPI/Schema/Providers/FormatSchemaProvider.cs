using Doctrina.xAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Schema.Providers
{
    public class FormatSchemaProvider : JSchemaGenerationProvider
    {
        public override JSchema GetSchema(JSchemaTypeGenerationContext context)
        {
            // customize the generated schema for these types to have a format
            if (context.ObjectType == typeof(IRI))
            {
                return CreateSchemaWithFormat(context.ObjectType, context.Required, "iri");
            }
            if (context.ObjectType == typeof(Mbox))
            {
                return CreateSchemaWithFormat(context.ObjectType, context.Required, "int64");
            }
            if (context.ObjectType == typeof(Guid))
            {
                return CreateSchemaWithFormat(context.ObjectType, context.Required, "uuid");
            }
            if (context.ObjectType == typeof(double))
            {
                return CreateSchemaWithFormat(context.ObjectType, context.Required, "double");
            }
            if (context.ObjectType == typeof(byte))
            {
                return CreateSchemaWithFormat(context.ObjectType, context.Required, "byte");
            }
            if (context.ObjectType == typeof(DateTime) || context.ObjectType == typeof(DateTimeOffset))
            {
                return CreateSchemaWithFormat(context.ObjectType, context.Required, "date-time");
            }

            // use default schema generation for all other types
            return null;
        }

        private JSchema CreateSchemaWithFormat(Type type, Required required, string format)
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            JSchema schema = generator.Generate(type, required != Required.Always);
            schema.Format = format;

            return schema;
        }

        private JSchema CreateSchemaWithFormatAndPattern(Type type, Required required, string format, string pattern)
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            JSchema schema = generator.Generate(type, required != Required.Always);
            schema.Format = format;
            schema.Pattern = pattern;

            return schema;
        }

        public static class Formats
        {
            public const string version = "^[0-9]+\\.[0-9]+\\.[0-9]+(?:-[0-9A-Za-z-]+)?$";
            public const string Uuid = "^[0-9a-fA-F]{8}(?:-[0-9a-fA-F]{4}){3}-[0-9a-fA-F]{12}$";

        }
    }
}
