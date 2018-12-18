using System;

namespace Doctrina.Core.Data
{
    public interface IDocumentEntity
    {
        Guid Id { get; set; }
        byte[] Content { get; set; }
        string ContentType { get; set; }
        string Tag { get; set; }
        DateTimeOffset LastModified { get; set; }
    }
}