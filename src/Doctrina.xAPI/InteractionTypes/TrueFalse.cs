using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.InteractionTypes
{
    public class TrueFalse : InteractionTypeBase
    {
        public TrueFalse()
        {
        }

        public TrueFalse(JToken jobj, ApiVersion version) : base(jobj, version)
        {
        }

        protected override InteractionType INTERACTION_TYPE => InteractionType.TrueFalse;
    }
}
