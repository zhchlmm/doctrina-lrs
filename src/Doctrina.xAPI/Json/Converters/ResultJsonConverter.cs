using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Doctrina.xAPI.Json.Converters
{
    public class ResultJsonConverter : ApiJsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(StatementsResult);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var target = new StatementsResult();
            serializer.Populate(reader, target);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var result = value as StatementsResult;

            writer.WriteStartObject();

            writer.WritePropertyName("statements");
            writer.WriteValue(result.Statements);

            writer.WritePropertyName("more");
            writer.WriteValue(result.More);

            writer.WriteEndObject();
        }
    }
}
