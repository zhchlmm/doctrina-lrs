using Newtonsoft.Json;
using System;

namespace Doctrina.xAPI.Json.Converters
{
    public class StrictStringConverter : ApiJsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                    var ser = new JsonSerializer();
                    return reader.Value;
                default:
                    throw new JsonSerializationException(string.Format("Token '{0}' of type {1} was not a JSON string", reader.Value, reader.TokenType));
            }
        }

        public override bool CanRead => true;

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;
    }
}
