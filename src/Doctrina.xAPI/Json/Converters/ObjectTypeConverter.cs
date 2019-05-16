using Newtonsoft.Json;
using System;

namespace Doctrina.xAPI.Json.Converters
{
    public class ObjectTypeConverter : ApiJsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ObjectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
            {
                throw new JsonSerializationException("Must be a string.");
            }

            var enumString = (string)reader.Value;

            try
            {
                ObjectType enumObjectType = enumString;
                return enumObjectType;
            }
            catch (Exception)
            {
                throw new JsonSerializationException($"'{enumString}' is not a valid objectType.");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            ObjectType objType = (ObjectType)value;
            writer.WriteValue(objType.ToString());
        }

        public override bool CanRead => true;
        public override bool CanWrite => true;
    }
}
