using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Core.Models
{
    [JsonObject]
    public class Attachment : JsonModel
    {
        /// <summary>
        /// Identifies the usage of this Attachment. 
        /// For example: one expected use case for Attachments is to include a "completion certificate". 
        /// An IRI corresponding to this usage MUST be coined, and used with completion certificate attachments.
        /// </summary>
        [JsonProperty("usageType",
            Required = Required.Always)]
        public Uri UsageType { get; set; }

        /// <summary>
        /// Display name (title) of this Attachment.
        /// </summary>
        [JsonProperty("display",
            Required = Required.DisallowNull)]
        public LanguageMap Display { get; set; }

        /// <summary>
        /// A description of the Attachment
        /// </summary>
        [JsonProperty("description",
            Required = Required.DisallowNull)]
        public LanguageMap Description { get; set; }

        /// <summary>
        /// The content type of the Attachment.
        /// </summary>
        [JsonProperty("contentType",
            Required = Required.Always)]
        public string ContentType { get; set; }

        /// <summary>
        /// The length of the Attachment data in octets. (Content-Type)
        /// 
        /// </summary>
        [JsonProperty("length",
            Required = Required.Always)]
        public int Length { get; set; }

        /// <summary>
        /// The SHA-2 hash of the Attachment data. (X-Experience-API-Hash)
        /// This property is always required, even if fileURL is also specified.
        /// </summary>
        [JsonProperty("sha2",
            Required = Required.Always)]
        public string SHA2 { get; set; }

        /// <summary>
        /// An IRL at which the Attachment data can be retrieved, or from which it used to be retrievable.
        /// </summary>
        [JsonProperty("fileUrl",
            Required = Required.DisallowNull)]
        public Uri FileUrl { get; set; }
    }
}
