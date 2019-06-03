using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.InteractionTypes
{
    public class Performance : InteractionTypeBase
    {
        protected override InteractionType INTERACTION_TYPE => InteractionType.Performance;

        public Performance() { }

        public Performance(JToken jobj, ApiVersion version) : base(jobj, version)
        {
            if (jobj["steps"] != null)
            {
                Steps = new InteractionComponentCollection(jobj.Value<JArray>("steps"), version);
            }
        }

        public InteractionComponentCollection Steps { get; set; }
    }
}
