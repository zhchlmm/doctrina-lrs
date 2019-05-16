using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.InteractionTypes
{
    public class Likert : InteractionTypeBase
    {
        protected override InteractionType INTERACTION_TYPE => InteractionType.Likert;

        public Likert()
        {
        }
        public Likert(JObject jobj, ApiVersion version) : base(jobj, version)
        {
            if (jobj["scale"] != null)
            {
                Scale = new InteractionComponentCollection(jobj.Value<JArray>("scale"), version);
            }
        }

        public InteractionComponentCollection Scale { get; set; }

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = base.ToJToken(version, format);

            return jobj;
        }
    }
}
