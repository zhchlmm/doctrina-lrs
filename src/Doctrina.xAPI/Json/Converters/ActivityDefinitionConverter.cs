using Doctrina.xAPI.InteractionTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Doctrina.xAPI.Json.Converters
{
    public class ActivityDefinitionConverter : ApiJsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ActivityDefinition).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jobj = JObject.Load(reader); // Crashed
            var target = new ActivityDefinition();

            JToken tokenInteractionType = jobj["interactionType"];
            if (tokenInteractionType != null)
            {
                if (tokenInteractionType.Type != JTokenType.String)
                {
                    throw new JsonSerializationException($"interactionType must be a string");
                }

                string strInteractionType = tokenInteractionType.Value<string>();
                if (strInteractionType.Any(x => char.IsUpper(x)))
                {
                    throw new JsonSerializationException($"interactionType '{strInteractionType}' contains uppercase charactors, which is not allowed.");
                }

                InteractionType interactionType = strInteractionType;

                //if (!interactionType.HasValue)
                //{
                //    throw new JsonSerializationException($"'{strInteractionType}' is not a valid interactionType. Path: '{tokenInteractionType.Path}'");
                //}

                target = interactionType.CreateInstance();

                serializer.Populate(jobj.CreateReader(), target);
                return target;
            }

            serializer.Populate(jobj.CreateReader(), target);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;
    }
}
