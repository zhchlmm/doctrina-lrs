using Newtonsoft.Json;

namespace Doctrina.xAPI.InteractionTypes
{
    public class Performance : InteractionTypeBase
    {
        protected override InteractionType INTERACTION_TYPE => InteractionType.Performance;

        [JsonProperty("steps")]
        public InteractionComponent[] Steps { get; set; }
    }
}
