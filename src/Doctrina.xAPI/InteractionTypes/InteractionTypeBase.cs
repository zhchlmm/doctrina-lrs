using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Doctrina.xAPI.InteractionTypes
{
    public abstract class InteractionTypeBase : ActivityDefinition
    {
        public InteractionTypeBase() { }
        public InteractionTypeBase(JsonString jsonString) : this(jsonString.ToJToken(), ApiVersion.GetLatest()) { }
        public InteractionTypeBase(JToken jobj, ApiVersion version) : base(jobj, version)
        {
            //if(jobj["interactionType"] != null)
            //{
            //    InteractionType = jobj.Value<string>("interactionType");
            //}

            var jCorrectResponsesPattern = jobj["correctResponsesPattern"];
            if (DisallowNullValue(jCorrectResponsesPattern))
            {
                if(AllowArray(jCorrectResponsesPattern))
                {
                    var correctResponsesPattern = new List<string>();
                    foreach(var jstring in jCorrectResponsesPattern)
                    {
                        if(AllowString(jstring))
                        {
                            correctResponsesPattern.Add(jstring.Value<string>());
                        }
                    }
                    CorrectResponsesPattern = correctResponsesPattern.ToArray();
                }
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
