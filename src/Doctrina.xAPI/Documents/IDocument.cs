using System;
using System.Net.Http.Headers;

namespace Doctrina.xAPI.Documents
{
    public interface IDocument
    {
        byte[] Content { get; set; }
        MediaTypeHeaderValue ContentType { get; set; }
        EntityTagHeaderValue ETag { get; set; }
        string Id { get; set; }
        DateTimeOffset? LastModified { get; set; }
    }
}