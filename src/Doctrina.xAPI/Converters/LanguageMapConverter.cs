using Doctrina.xAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Converters
{
    public class LanguageMapConverter : JsonConverter
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
                    var jvalue = property.Value;
                    string value = jvalue.Value<string>();
                    if (value == null)
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

                    if(!maps.ContainsKey(property.Name))
                        maps.Add(property.Name, value);
                }

                return maps;
            }

            throw new JsonSerializationException("Must be a Object");
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;
    }
}
