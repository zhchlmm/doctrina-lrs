using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Doctrina.xAPI.Json
{
    public abstract class ApiJsonConverter : JsonConverter
    {
        public override abstract object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer);
        public override abstract void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer);

        public void IsString(JToken token)
        {
            if (token.Type != JTokenType.String)
            {
                throw new JsonSerializationException("Is not a valid string.");
            }
        }

        public void IsNumber(JToken token)
        {
            if (token.Type != JTokenType.Integer && token.Type != JTokenType.Float)
            {
                throw new JsonSerializationException("Is not a valid number.");
            }
        }
    }
}
