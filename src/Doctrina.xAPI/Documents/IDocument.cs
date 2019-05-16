using System;

namespace Doctrina.xAPI.Documents
{
    public interface IDocument
    {
        byte[] Content { get; set; }
        string ContentType { get; set; }
        string Tag { get; set; }
        DateTimeOffset? LastModified { get; set; }
    }
}