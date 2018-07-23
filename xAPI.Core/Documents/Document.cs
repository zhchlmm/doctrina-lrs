using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Core.Documents
{
    public abstract class Document
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("etag")]
        public string ETag { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("content")]
        public byte[] Content { get; set; }
    }
}
