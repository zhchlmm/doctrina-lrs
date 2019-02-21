using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Doctrina.xAPI.Json.Converters
{
    public class StatementObjectConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(typeof(StatementObjectBase));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            string path = reader.Path;

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
            else if (jobj["id"] != null)
            {
                // If objectType is not defined, but id is, it's an activity
                objType = ObjectType.Activity;
            }
            else
            {
                // Do not assume (Data.md#2.4.4.2.s1.b1)
                throw new JsonSerializationException("Statements that use an Agent or Group as an Object MUST specify an 'objectType' property.");
            }

            // If Statement's SubStatement object
            if (path == "object.object")
            {
                return ReadSubStatementObject(serializer, jobj, objType);
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
                case ObjectType.SubStatement:
                    target = new SubStatement();
                    break;
                case ObjectType.StatementRef:
                    target = new StatementRef();
                    break;
                default:
                    throw new NullReferenceException($"objectType '{objType}' is not valid. Path '{jobj.Path}'.");
            }

            serializer.Populate(jobj.CreateReader(), target);
            return target;
        }

        private object ReadSubStatementObject(Newtonsoft.Json.JsonSerializer serializer, JObject jobj, ObjectType objType)
        {
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
            //serializer.Populate(jobj.CreateReader(), target);
            //return target;
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;
    }
}
