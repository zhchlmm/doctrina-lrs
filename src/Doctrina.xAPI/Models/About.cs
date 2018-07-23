using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Models
{
    [JsonObject]
    public class About
    {
        [JsonProperty("version")]
        public IEnumerable<string> Version { get; set; }

        [JsonProperty("extensions")]
        public Extensions Extensions { get; set; }
    }
}
