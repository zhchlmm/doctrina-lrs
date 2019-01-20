using Doctrina.xAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Json.Converters
{
    public class ObjectTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ObjectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var enumString = (string)reader.Value;

            ObjectType? enumObjectType = null;
            var members = typeof(ObjectType).GetEnumValues();
            foreach (var enumValue in members)
            {
                var memberType = enumValue.GetType();
                var memberInfo = memberType.GetMember(enumValue.ToString());
                var attribute = (EnumMemberAttribute)memberInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false).FirstOrDefault();
                if (attribute == null)
                    continue;

                if (attribute.Value == enumString)
                {
                    // Match
                    enumObjectType = (ObjectType)enumValue;
                    break;
                }
            }

            if (enumObjectType.HasValue)
                return enumObjectType.Value;

            return Enum.Parse(typeof(ObjectType), enumString, true);
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var memberType = value.GetType();
            var memberInfo = memberType.GetMember(value.ToString());
            var enumMemberAttributes = memberInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false);

            if (enumMemberAttributes.Any())
            {
                var attribute = (EnumMemberAttribute)enumMemberAttributes[0];
                writer.WriteValue(attribute.Value);
            }
            else
            {
                writer.WriteValue(nameof(value));
            }
        }

        public override bool CanRead => true;
        public override bool CanWrite => true;
    }
}
