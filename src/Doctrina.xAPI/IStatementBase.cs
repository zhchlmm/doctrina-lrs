using System;

namespace Doctrina.xAPI
{
    public interface IStatementBase
    {
        Agent Actor { get; set; }
        AttachmentCollection Attachments { get; set; }
        Context Context { get; set; }
        Result Result { get; set; }
        IObjectType Object { get; set; }
        DateTimeOffset? Timestamp { get; set; }
        Verb Verb { get; set; }
    }
}