using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.InteractionTypes
{
    public class Matching : InteractionTypeBase
    {
        protected override InteractionType INTERACTION_TYPE => InteractionType.Matching;

        public Matching()
        {
        }
        public Matching(JObject jobj, ApiVersion version) : base(jobj, version)
        {
            if (jobj["source"] != null)
            {
                Source = new InteractionComponentCollection(jobj.Value<JArray>("source"), version);
            }

            if (jobj["target"] != null)
            {
                Target = new InteractionComponentCollection(jobj.Value<JArray>("target"), version);
            }
        }

        [JsonProperty("source")]
        public InteractionComponentCollection Source { get; set; }

        [JsonProperty("target")]
        public InteractionComponentCollection Target { get; set; }
    }
}
