using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Converters
{
    public class XAPIVersionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(XAPIVersion);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var jtoken = JToken.Load(reader);
            if (jtoken.Type != JTokenType.String)
                throw new JsonSerializationException("This is not a string");

            return new XAPIVersion(jtoken.ToObject<string>());
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override bool CanWrite => true;
        public override bool CanRead => true; 
    }
}
