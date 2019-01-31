using System;

namespace Doctrina.Domain.Entities.Documents
{
    public interface IDocumentEntity
    {
        byte[] Content { get; set; }
        string ContentType { get; set; }
        string Tag { get; set; }
        DateTimeOffset LastModified { get; set; }
    }
}