using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Xml;

namespace Doctrina.xAPI.Json.Converters
{
    public class TimeSpanConverter : JsonConverter
    {
        private string[] tests = { "P1DT12H", "PT4H35M59.14S", "P16559.14S", "P3Y1M29DT4H35M59.14S", "P3Y", "P1W" };
        private const string ISO_DURATION_FORMAT = @"^P(?=\w*\d)(?:\d+Y|Y)?(?:\d+M|M)?(?:\d+W|W)?(?:\d+D|D)?(?:T(?:\d+H|H)?(?:\d+M|M)?(?:\d+(?:\.\d{1,2})?S|S)?)?$";

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var ts = (TimeSpan)value;
            string tsString = XmlConvert.ToString(ts);
            serializer.Serialize(writer, tsString);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            // TODO: Parsing P1W fails
            string value = serializer.Deserialize<string>(reader);

            try
            {
                return XmlConvert.ToTimeSpan(value);
            }
            catch (Exception)
            {
                throw new JsonSerializationException($"'{value}' is not a valid ISO 8601 duration");
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeSpan) || objectType == typeof(TimeSpan?);
        }
    }
}
