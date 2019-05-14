using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.InteractionTypes
{
    public abstract class InteractionTypeBase : ActivityDefinition
    {
        public InteractionTypeBase() { }
        public InteractionTypeBase(string jsonString) : this(JObject.Parse(jsonString)){ }
        public InteractionTypeBase(JObject jobj) : this(jobj, ApiVersion.GetLatest()){ }
        public InteractionTypeBase(JObject jobj, ApiVersion version)
            : base(jobj, version)
        {
        }

        public override Iri Type { get => new Iri("http://adlnet.gov/expapi/activities/cmi.interaction"); set => base.Type = value; }

        protected abstract InteractionType INTERACTION_TYPE { get; }

        public InteractionType InteractionType { get { return INTERACTION_TYPE; } }

        public string[] CorrectResponsesPattern { get; set; }

        public override JObject ToJObject(ApiVersion version, ResultFormat format)
        {
            var obj = base.ToJObject(version, format);
            obj.Add("interactionType", (string)InteractionType);
            obj.Add("correctResponsesPattern", JToken.FromObject(CorrectResponsesPattern));
            return obj;
        }
    }
}
