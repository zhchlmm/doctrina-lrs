using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.InteractionTypes
{
    public class InteractionComponent : JsonModel<JObject>
    {
        public InteractionComponent()
        {
        }

        public InteractionComponent(string jsonString)
            : this(JObject.Parse(jsonString))
        {
        }

        public InteractionComponent(JObject jobj)
           : this(jobj, ApiVersion.GetLatest())
        {
        }

        public InteractionComponent(JObject jobj, ApiVersion version)
        {
            var id = jobj["id"];
            if (id != null && AllowString(id))
            {
                Id = jobj.Value<string>("id");
            }

            var desc = jobj["description"];
            if (desc != null && DisallowNull(desc))
            {
                Description = new LanguageMap(desc.Value<JObject>(), version);
            }
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("description")]
        public LanguageMap Description { get; set; }

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = new JObject();
            if (!string.IsNullOrEmpty(Id))
            {
                jobj["id"] = Id;
            }

            if (Description != null)
            {
                jobj["description"] = Description.ToJToken(version, format);
            }
            return jobj;
        }
    }
}
