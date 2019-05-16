using System;

namespace Doctrina.xAPI
{
    public interface IStatement
    {
        Guid? Id { get; set; }
        Agent Actor { get; set; }
        Attachment[] Attachments { get; set; }
        Agent Authority { get; set; }
        Context Context { get; set; }
        IObjectType Object { get; set; }
        Result Result { get; set; }
        DateTimeOffset? Stored { get; set; }
        DateTimeOffset? Timestamp { get; set; }
        Verb Verb { get; set; }
        ApiVersion Version { get; set; }
    }
}