using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;

namespace Doctrina.xAPI.Documents
{
    public abstract class Document
    {
        public string Id { get; set; }

        public EntityTagHeaderValue ETag { get; set; }

        /// <summary>
        /// Last Modified
        /// </summary>
        public DateTimeOffset? LastModified { get; set; }

        public MediaTypeHeaderValue ContentType { get; set; }

        public byte[] Content { get; set; }
    }
}
