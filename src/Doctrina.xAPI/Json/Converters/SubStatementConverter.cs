using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Doctrina.xAPI.Json.Converters
{
    public class SubStatementConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SubStatement);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jobj = JObject.Load(reader);

            ObjectType objType = ObjectType.Agent;
            var jobjectType = jobj["objectType"];
            if (jobjectType != null)
            {
                if (jobjectType.Type != JTokenType.String)
                    throw new JsonSerializationException("objectType must be a string");

                objType = jobj["objectType"].Value<string>();
            }

            // If Statement Object
            var target = objType.CreateInstance();
            return serializer.Deserialize(jobj.CreateReader(), target.GetType());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead => true;
    }
}
