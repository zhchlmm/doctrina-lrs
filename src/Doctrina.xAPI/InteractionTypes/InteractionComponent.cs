using Newtonsoft.Json;

namespace Doctrina.xAPI.InteractionTypes
{
    public class InteractionComponent
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("description")]
        public LanguageMap Description { get; set; }
    }
}
