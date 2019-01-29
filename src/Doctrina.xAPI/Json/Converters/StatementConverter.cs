using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Doctrina.xAPI.Json.Converters
{
    public class StatementConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Statement);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var target = new Statement();
            var jobj = JObject.Load(reader);

            serializer.Populate(jobj.CreateReader(), target);

            if(target.Context != null)
            {
                if (!string.IsNullOrWhiteSpace(target.Context.Revision)
                && target.Object.ObjectType != ObjectType.Activity)
                {
                    throw new JsonSerializationException("A Statement cannot contain both a 'revision' property in its 'context' property and have the value of the 'object' property's 'objectType' be anything but 'Activity'");
                }

                if (!string.IsNullOrWhiteSpace(target.Context.Platform)
                    && target.Object.ObjectType != ObjectType.Activity)
                {
                    throw new JsonSerializationException("A Statement cannot contain both a 'platform' property in its 'context' property and have the value of the 'object' property's 'objectType' be anything but 'Activity'");
                }
            }

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead => true;

        public override bool CanWrite => false; 
    }
}
