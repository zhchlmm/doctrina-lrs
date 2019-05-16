using System;

namespace Doctrina.xAPI.Documents
{
    public abstract class Document : IDocument
    {
        /// <summary>
        /// Gets or sets the opaque quoted string.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Last Modified
        /// </summary>
        public DateTimeOffset? LastModified { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }
    }
}
