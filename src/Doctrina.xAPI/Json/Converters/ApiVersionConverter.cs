using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Doctrina.xAPI.Json.Converters
{
    public class ApiVersionConverter : ApiJsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ApiVersion);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var jtoken = JToken.Load(reader);
            if (jtoken.Type != JTokenType.String)
                throw new JsonSerializationException("Version must be a string");

            string strVersion = jtoken.ToObject<string>();

            if (ApiVersion.TryParse(strVersion, out ApiVersion version))
            {
                return version;
            }

            throw new JsonSerializationException("");
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override bool CanWrite => true;
        public override bool CanRead => true;
    }
}
