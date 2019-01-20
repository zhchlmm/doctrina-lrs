using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

namespace Doctrina.xAPI.Json.Converters
{
    public class LanguageMapJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(LanguageMap);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if(reader.TokenType == JsonToken.StartObject)
            {
                JObject jobj = JObject.Load(reader);

                //if (existingValue == null)
                //    throw new NullReferenceException($"Required property '' expects a non-null value. Path '{reader.Path}'.");

                var props = jobj.Properties();
                var maps = new LanguageMap();
                foreach (var property in props)
                {
                    JToken propValue = property.Value;
                    if (propValue.Type != JTokenType.String)
                        throw new JsonSerializationException($"Language key '{property.Name}' value '{propValue.ToString()}' must be a string.");

                    string description = propValue.Value<string>();
                    if (description == null)
                        throw new JsonSerializationException($"Required property '{property.Name}' expects a non-null value. Path '{reader.Path}'.");

                    try
                    {
                        if(property.Name == "und")
                        {
                            // Undetermined
                        }
                        else
                        {
                            var culture = CultureInfo.GetCultureInfo(property.Name);
                        }
                    }
                    catch (Exception)
                    {
                        throw new JsonSerializationException($"Invalid language code `{property.Name}`. Path: `{reader.Path}`");
                    }

                    if (maps.ContainsKey(property.Name))
                        throw new JsonSerializationException($"Duplicate language key '{property.Name}'. Path '{reader.Path}'");

                    maps.Add(property.Name, description);
                }

                return maps;
            }

            throw new JsonSerializationException("Must be a Object");
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            //serializer.Serialize(writer, value);
            var langMap = value as LanguageMap;
            if (langMap == null)
                return;

            //writer.WriteStartObject();
            //foreach (var lang in langMap)
            //{
            //    string name = lang.Key;
            //    string text = lang.Value;
            //    writer.WritePropertyName(name);
            //    writer.WriteValue(text);
            //}
            //writer.WriteEndObject();

            var jobj = new JObject();
            foreach(var lang in langMap){
                jobj.Add(lang.Key, lang.Value);
            }
            jobj.WriteTo(writer);
        }

        public override bool CanRead => true;
        public override bool CanWrite => false;
    }
}
