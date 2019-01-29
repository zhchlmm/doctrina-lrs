using Newtonsoft.Json;

namespace Doctrina.xAPI.InteractionTypes
{
    public class Likert : InteractionTypeBase
    {
        protected override InteractionType INTERACTION_TYPE => InteractionType.Likert;

        [JsonProperty("scale")]
        public InteractionComponent[] Scale { get; set; }
    }
}
