using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doctrina.xAPI.Models;
using Doctrina.xAPI.Models.InteractionTypes;
using System.Runtime.Serialization;

namespace Doctrina.xAPI.Converters
{
    public class ActivityDefinitionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ActivityDefinition).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            JObject jobj = JObject.Load(reader); // Crashed
            var target = new ActivityDefinition();

            var jinteractionType = jobj["interactionType"];
            if (jinteractionType != null)
            {
                string strInteractionType = jinteractionType.Value<string>();
                if (strInteractionType.Any(x => Char.IsUpper(x)))
                {
                    throw new JsonSerializationException($"interactionType '{strInteractionType}' contains uppercase charactors, which is not allowed.");
                }

                InteractionType? interactionType = null;
                var members = typeof(InteractionType).GetEnumValues();
                foreach (var enumValue in members)
                {
                    var memberType = enumValue.GetType();
                    var memberInfo = memberType.GetMember(enumValue.ToString());
                    var attribute = (EnumMemberAttribute)memberInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false).FirstOrDefault();
                    if (attribute == null)
                        continue;

                    if (attribute.Value == strInteractionType)
                    {
                        // Match
                        interactionType = (InteractionType)enumValue;
                        break;
                    }
                }

                if (!interactionType.HasValue)
                { }



                //if (Enum.TryParse(strInteractionType, true, out interactionType))
                //{
                switch (interactionType.Value)
                {
                    case InteractionType.TrueFalse:
                        target = new TrueFalse();
                        break;
                    case InteractionType.Choice:
                        target = new Choice();
                        break;
                    case InteractionType.FillIn:
                        target = new FillIn();
                        break;
                    case InteractionType.LongFillIn:
                        target = new LongFillIn();
                        break;
                    case InteractionType.Matching:
                        target = new Matching();
                        break;
                    case InteractionType.Performance:
                        target = new Performance();
                        break;
                    case InteractionType.Sequencing:
                        target = new Sequencing();
                        break;
                    case InteractionType.Likert:
                        target = new Likert();
                        break;
                    case InteractionType.Numeric:
                        target = new Numeric();
                        break;
                    case InteractionType.Other:
                        target = new Other();
                        break;
                    default:
                        throw new JsonSerializationException($"'{strInteractionType}' is not a valid interactionType. Path '{reader.Path}'");
                }

                serializer.Populate(jobj.CreateReader(), target);
                return target;
                //}

                //throw new JsonSerializationException($"'{strInteractionType}' is not a valid interactionType. Path '{reader.Path}'");
            }

            serializer.Populate(jobj.CreateReader(), target);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;
    }
}
