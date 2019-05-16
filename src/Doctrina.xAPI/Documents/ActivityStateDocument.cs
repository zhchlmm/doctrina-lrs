using Newtonsoft.Json;
using System;

namespace Doctrina.xAPI.Documents
{
    public class ActivityStateDocument : Document, IActivityStateDocument
    {
        [JsonProperty("stateId")]
        public string StateId { get; set; }

        [JsonProperty("activity")]
        public Activity Activity { get; set; }

        [JsonProperty("activity")]
        public Agent Agent { get; set; }

        [JsonProperty("registration")]
        public Guid? Registration { get; set; }
    }
}
