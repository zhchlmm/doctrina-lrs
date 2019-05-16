﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Doctrina.xAPI
{
    [JsonObject]
    public class Attachment : JsonModel
    {
        public Attachment() { }
        public Attachment(string jsonString) :this(JObject.Parse(jsonString)) { }
        public Attachment(JObject jobj) :this(jobj, ApiVersion.GetLatest()) { }
        public Attachment(JObject jobj, ApiVersion version)
        {
            if(jobj["usageType"] != null)
            {
                UsageType = jobj.Value<Iri>("usageType");
            }

            if(jobj["display"] != null)
            {
                Display = new LanguageMap(jobj.Value<JObject>("display"), version);
            }

            if (jobj["description"] != null)
            {
                Description = new LanguageMap(jobj.Value<JObject>("description"), version);
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
                FileUrl = jobj.Value<Uri>("fileUrl");
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
        /// 
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

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var obj = new JObject();
            obj["usageType"] = UsageType.ToString();

            if (Display != null)
            {
                obj["dispay"] = Display.ToJToken(version, format);
            }
            
            if(Description != null)
            {
                obj["description"] = Description.ToJToken(version, format);
            }

            obj["contentType"] = ContentType;

            obj["length"] = Length;

            obj["sha2"] = SHA2;

            if(FileUrl != null)
            {
                obj["fileUrl"] = FileUrl;
            }

            return obj;
        }
    }
}
