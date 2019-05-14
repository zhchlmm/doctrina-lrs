using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.InteractionTypes
{
    public class Likert : InteractionTypeBase
    {
        protected override InteractionType INTERACTION_TYPE => InteractionType.Likert;

        [JsonProperty("scale")]
        public InteractionComponent[] Scale { get; set; }

        public override JObject ToJObject(ApiVersion version, ResultFormat format)
        {
            var jobj = base.ToJObject(version, format);

            return jobj;
        }
    }
}
