using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Core.Models;

namespace xAPI.Core.Documents
{
    public class StateDocument : Document
    {
        [JsonProperty("activity")]
        public Activity Activity { get; set; }

        [JsonProperty("activity")]
        public Agent Agent { get; set; }

        [JsonProperty("registration")]
        public Guid? Registration { get; set; }
    }
}
