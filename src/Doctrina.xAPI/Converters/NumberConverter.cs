using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Converters
{
    public class NumberConverter : JsonConverter
    {
        private readonly Type[] _typesNotToReadAsString = {
            typeof(float), typeof(float?),
            typeof(int), typeof(int?),
            typeof(decimal), typeof(decimal?),
            typeof(double), typeof(double?),
        };

        public override bool CanConvert(Type objectType)
        {
            return _typesNotToReadAsString.Any(t => t.IsAssignableFrom(objectType));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);

            if (_typesNotToReadAsString.Contains(objectType) && token.Type == JTokenType.String)
            {
                throw new JsonSerializationException($"A number is required.");
            }

            return token.ToObject(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite
        {
            get { return false; }
        }
    }
}
