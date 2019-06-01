using Doctrina.xAPI.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Doctrina.xAPI
{
    [JsonObject]
    public class Attachment : JsonModel
    {
        public Attachment() { }
        public Attachment(JsonString jsonString) : this(jsonString.ToJToken()) { }
        public Attachment(JToken jobj) : this(jobj, ApiVersion.GetLatest()) { }
        public Attachment(JToken jobj, ApiVersion version)
        {
            if (!AllowObject(jobj))
            {
                return;
            }

            if (jobj["usageType"] != null)
            {
                UsageType = new Iri(jobj.Value<string>("usageType"));
            }

            if (jobj["display"] != null)
            {
                Display = new LanguageMap(jobj["display"], version);
            }

            if (jobj["description"] != null)
            {
                Description = new LanguageMap(jobj["description"], version);
            }

            if (jobj["contentType"] != null)
            {
                ContentType = jobj.Value<string>("contentType");
            }

            if (jobj["length"] != null)
            {
                Length = jobj.Value<int>("length");
            }

            if (jobj["sha2"] != null)
            {
                SHA2 = jobj.Value<string>("sha2");
            }

            if (jobj["fileUrl"] != null)
            {
                FileUrl = new Uri(jobj.Value<string>("fileUrl"));
            }
        }

        /// <summary>
        /// Identifies the usage of this Attachment. 
        /// For example: one expected use case for Attachments is to include a "completion certificate". 
        /// An IRI corresponding to this usage MUST be coined, and used with completion certificate attachments.
        /// </summary>
        public Iri UsageType { get; set; }

        /// <summary>
        /// Display name (title) of this Attachment.
        /// </summary>
        public LanguageMap Display { get; set; }

        /// <summary>
        /// A description of the Attachment
        /// </summary>
        public LanguageMap Description { get; set; }

        /// <summary>
        /// The content type of the Attachment.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The length of the Attachment data in octets. (Content-Type)
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// The SHA-2 hash of the Attachment data. (X-Experience-API-Hash)
        /// This property is always required, even if fileURL is also specified.
        /// </summary>
        public string SHA2 { get; set; }

        /// <summary>
        /// An IRL at which the Attachment data can be retrieved, or from which it used to be retrievable.
        /// </summary>
        public Uri FileUrl { get; set; }

        /// <summary>
        /// The byte array of Attachment data. 
        /// </summary>
        [JsonIgnore]
        public byte[] Payload { get; set; }


        /// <summary>
        /// Sets the Attachment data payload.
        /// </summary>
        /// <param name="bytes">The bytes representing the payload.</param>
        public void SetPayload(byte[] bytes)
        {
            Payload = bytes;

            if (!string.IsNullOrEmpty(SHA2))
            {
                SHA2 = SHAHelper.SHA2.ComputeHash(bytes);
            }

            if(Length <= 0)
            {
                Length = bytes.Length;
            }
        }

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var obj = new JObject();

            obj["usageType"] = UsageType.ToString();

            if (Display != null)
            {
                obj["dispay"] = Display.ToJToken(version, format);
            }

            if (Description != null)
            {
                obj["description"] = Description.ToJToken(version, format);
            }

            obj["contentType"] = ContentType;

            obj["length"] = Length;

            obj["sha2"] = SHA2;

            if (FileUrl != null)
            {
                obj["fileUrl"] = FileUrl.ToString();
            }

            return obj;
        }
    }
}
