using Newtonsoft.Json;

namespace Doctrina.xAPI.InteractionTypes
{
    public class Matching : InteractionTypeBase
    {
        protected override InteractionType INTERACTION_TYPE => InteractionType.Matching;

        [JsonProperty("source")]
        public InteractionComponent[] Source { get; set; }

        [JsonProperty("target")]
        public InteractionComponent[] Target { get; set; }
    }
}
