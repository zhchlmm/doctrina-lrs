using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Json.Converters
{
    /// <summary>
    /// Ensures DateTime is well formed
    /// </summary>
    public class DateTimeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime)
                || objectType == typeof(DateTimeOffset);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            string strDateTime = reader.Value as string;

            if (strDateTime.EndsWith("-00:00")
                || strDateTime.EndsWith("-0000")
                || strDateTime.EndsWith("-00"))
            {
                throw new JsonSerializationException($"'{strDateTime}' does not allow an offset of -00:00, -0000, -00");
            }

            if (DateTime.TryParse(strDateTime, out DateTime result))
            {
                return result;
            }
            else
            {
                throw new JsonSerializationException($"'{strDateTime}' is not a well formed DateTime string.");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var dateTime = value as DateTime?;
            var jvalue = new JValue(dateTime?.ToString("o"));
            jvalue.WriteTo(writer);
        }

        public override bool CanRead => true;
        public override bool CanWrite => true;
    }
}
