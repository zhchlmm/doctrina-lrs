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
    /// Ensures Uri is well formed
    /// </summary>
    public class UriJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Uri);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string uriString = reader.ReadAsString();
            if(Uri.IsWellFormedUriString(uriString, UriKind.RelativeOrAbsolute))
            {
                return new Uri(uriString);
            }

            throw new JsonSerializationException($"'{uriString}' is not a well formed Uri string.");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var uri = value as Uri;
            var jvalue = new JValue(uri.ToString());
            jvalue.WriteTo(writer);
            //writer.WriteValue(uri.ToString());
        }

        public override bool CanRead => true;
        public override bool CanWrite => true;
    }
}
