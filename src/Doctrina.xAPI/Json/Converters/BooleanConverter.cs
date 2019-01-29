using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Doctrina.xAPI.Json.Converters
{
    public class BooleanConverter : JsonConverter
    {
        private readonly Type[] _typesNotToReadAsString = {
            typeof(bool), typeof(bool?),
        };

        public override bool CanConvert(Type objectType)
        {
            return _typesNotToReadAsString.Any(t => t.IsAssignableFrom(objectType));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);

            if (_typesNotToReadAsString.Contains(objectType) && token.Type != JTokenType.Boolean)
            {
                throw new JsonSerializationException($"A boolean value is required.");
            }

            return token.ToObject(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }

        public override bool CanWrite
        {
            get { return false; }
        }
    }
}
