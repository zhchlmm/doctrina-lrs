using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Doctrina.xAPI.Json.Converters
{
    public class StatementJsonConverter : ApiJsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Statement);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var target = new Statement();
            var jobj = JObject.Load(reader);

            serializer.Populate(jobj.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead => true;

        public override bool CanWrite => false;
    }
}
