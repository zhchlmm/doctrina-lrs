using Doctrina.xAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Json.Converters
{
    public class AgentJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Agent).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            JObject jobj = JObject.Load(reader);

            // Default to agent
            ObjectType objType = ObjectType.Agent;
            var jObjectType = jobj["objectType"];
            if (jObjectType != null)
            {
                string strObjectType = jObjectType.Value<string>();
                if (!Enum.TryParse(strObjectType, out objType))
                    throw new JsonSerializationException($"'{strObjectType}' is not valid. Path: '{jObjectType.Path}'");

                if(!Enum.IsDefined(typeof(ObjectType), objType))
                    throw new JsonSerializationException($"'{strObjectType}' is not valid. Path: '{jObjectType.Path}'");
            }

            if (objType == ObjectType.Agent)
            {
                var agent = new Agent();
                serializer.Populate(jobj.CreateReader(), agent);
                return agent;
            }

            if (objType == ObjectType.Group)
            {
                var group = new Group();
                serializer.Populate(jobj.CreateReader(), group);
                return group;
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var xapiSerializer = (XAPISerializer)serializer;
            var format = xapiSerializer.ResultFormat;
            writer.WriteStartObject();
            // TODO: Write Agent result based on result format.
            writer.WriteEndObject();
        }

        public override bool CanWrite => false;
    }
}
