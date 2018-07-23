using System;

namespace Doctrina.Core.Persistence.Models
{
    public interface IDocumentEntity
    {
        Guid Id { get; set; }
        byte[] Content { get; set; }
        string ContentType { get; set; }
        string ETag { get; set; }
        DateTime Timestamp { get; set; }
    }
}