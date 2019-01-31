using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Doctrina.xAPI.Json.Converters
{
    public class ExtensionsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Extensions));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            JObject jobj = JObject.Load(reader);
            var extensions = new Extensions();
            foreach(var pair in jobj)
            {
                try
                {
                    var uri = new Uri(pair.Key);
                    if(pair.Value.Type == JTokenType.Null)
                    {
                        extensions.Add(uri, null);
                    }
                    else
                    {
                        extensions.Add(uri, pair.Value);
                    }
                }
                catch (Exception)
                {
                    throw new JsonSerializationException($"Failed to deserialize '{pair.Key}'. Path: '{pair.Value.Path}'");
                }
            }
            return extensions;
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var extensions = value as Extensions;
            writer.WriteStartObject();
            foreach(var pair in extensions)
            {
                writer.WritePropertyName(pair.Key.ToString());
                writer.WriteValue(pair.Value);
            }
            writer.WriteEndObject();
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanRead
        {
            get { return true; }
        }
    }
}
