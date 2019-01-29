using Newtonsoft.Json;
using System;

namespace Doctrina.xAPI
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
        public Iri UsageType { get; set; }

        /// <summary>
        /// Display name (title) of this Attachment.
        /// </summary>
        [JsonProperty("display",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize)]
        public LanguageMap Display { get; set; }

        /// <summary>
        /// A description of the Attachment
        /// </summary>
        [JsonProperty("description",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize)]
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
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public Uri FileUrl { get; set; }

        /// <summary>
        /// The byte array of Attachment data. 
        /// </summary>
        [JsonIgnore]
        public byte[] Payload { get; private set; }


        /// <summary>
        /// Sets the Attachment data payload.
        /// </summary>
        /// <param name="bytes">The bytes representing the payload.</param>
        public void SetPayload(byte[] bytes)
        {
            Payload = bytes;

            if (UsageType == new Iri("http://adlnet.gov/expapi/attachments/signature")
                && ContentType == "application/octet-stream")
            {
                // Verify signatures are well formed

                // Decode the JWS signature, and load the signed serialization of the Statement from the JWS signature payload.

                // Validate that the original Statement is logically equivalent to the received Statement.

                // If the JWS header includes an X.509 certificate, validate the signature against that certificate as defined in JWS.

                // Validate that the signature requirements outlined above have been met.
            }
        }
    }
}
