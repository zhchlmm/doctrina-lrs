using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Core.Converters
{
    public class BooleanConverter : JsonConverter
    {
        private readonly Type[] _typesNotToReadAsString = {
            typeof(bool), typeof(bool?),
        };

        public override bool CanConvert(Type objectType)
        {
            return _typesNotToReadAsString.Any(t => t.IsAssignableFrom(objectType));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);

            if (_typesNotToReadAsString.Contains(objectType) && token.Type != JTokenType.Boolean)
            {
                throw new JsonSerializationException($"A boolean is required.");
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
