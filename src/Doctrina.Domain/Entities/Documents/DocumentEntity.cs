using System;

namespace Doctrina.Domain.Entities.Documents
{
    /// <summary>
    /// Represents a stored document
    /// </summary>
    public class DocumentEntity : IDocumentEntity
    {
        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        public string Tag { get; set; }

        public DateTimeOffset LastModified { get; set; }
    }
}
