using Doctrina.xAPI.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Doctrina.xAPI.InteractionTypes
{
    public abstract class InteractionTypeBase : ActivityDefinition
    {
        public InteractionTypeBase() { }

        public InteractionTypeBase(JsonString jsonString) : this(jsonString.ToJToken(), ApiVersion.GetLatest()) { }

        public InteractionTypeBase(JToken interactionType, ApiVersion version) : base(interactionType, version)
        {
            var correctResponsesPattern = interactionType["correctResponsesPattern"];
            if (correctResponsesPattern != null)
            {
                GuardType(correctResponsesPattern, JTokenType.Array);

                var stringList = new List<string>();
                foreach (var jstring in correctResponsesPattern)
                {
                    GuardType(jstring, JTokenType.String);
                    stringList.Add(jstring.Value<string>());
                }
                CorrectResponsesPattern = stringList.ToArray();
            }
        }

        public override Iri Type { get => new Iri("http://adlnet.gov/expapi/activities/cmi.interaction"); set => base.Type = value; }

        protected abstract InteractionType INTERACTION_TYPE { get; }

        public InteractionType InteractionType { get { return INTERACTION_TYPE; } }

        public string[] CorrectResponsesPattern { get; set; }

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            var obj = base.ToJToken(version, format);
            if (InteractionType != null)
            {
                obj["interactionType"] = (string)InteractionType;
            }
            if (CorrectResponsesPattern != null)
            {
                obj["correctResponsesPattern"] = JArray.FromObject(CorrectResponsesPattern);
            }
            return obj;
        }
    }
}
