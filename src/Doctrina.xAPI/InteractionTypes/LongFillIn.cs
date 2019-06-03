using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.InteractionTypes
{
    public class LongFillIn : InteractionTypeBase
    {
        protected override InteractionType INTERACTION_TYPE => InteractionType.LongFillIn;

        public LongFillIn()
        {
        }
        public LongFillIn(JToken jobj, ApiVersion version) : base(jobj, version)
        {
        }
    }
}
