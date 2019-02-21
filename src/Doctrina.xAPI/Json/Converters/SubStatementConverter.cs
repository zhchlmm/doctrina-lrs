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

                string strObjectType = jobj["objectType"].Value<string>();

                if (!Enum.TryParse(strObjectType, out objType))
                    throw new JsonSerializationException($"'{strObjectType}' is not valid. Path: '{reader.Path}'");
            }

            StatementObjectBase target = null;

            // If Statement Object
            switch (objType)
            {
                case ObjectType.Activity:
                    target = new Activity();
                    break;
                case ObjectType.Agent:
                    target = new Agent();
                    break;
                case ObjectType.Group:
                    target = new Group();
                    break;
                case ObjectType.StatementRef:
                    target = new StatementRef();
                    break;
                default:
                    throw new NullReferenceException($"objectType '{objType}' is not valid at path '{jobj.Path}'");
            }

            return serializer.Deserialize(jobj.CreateReader(), target.GetType());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead => true;
    }
}
