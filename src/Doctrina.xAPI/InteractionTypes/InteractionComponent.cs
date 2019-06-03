using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.InteractionTypes
{
    public class InteractionComponent : JsonModel
    {
        public InteractionComponent() {}

        public InteractionComponent(JsonString jsonString) : this(jsonString.ToJToken(), ApiVersion.GetLatest()) {}

        public InteractionComponent(JToken jobj, ApiVersion version)
        {
            var id = jobj["id"];
            if (id != null && AllowString(id))
            {
                Id = id.Value<string>();
            }

            var desc = jobj["description"];
            if (DisallowNullValue(desc))
            {
                Description = new LanguageMap(desc, version);
            }
        }

        public string Id { get; set; }

        public LanguageMap Description { get; set; }

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
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
