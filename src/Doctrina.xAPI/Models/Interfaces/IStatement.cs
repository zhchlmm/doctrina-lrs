using System;

namespace Doctrina.xAPI.Models
{
    public interface IStatement
    {
        Guid? Id { get; set; }
        Agent Actor { get; set; }
        Attachment[] Attachments { get; set; }
        Agent Authority { get; set; }
        Context Context { get; set; }
        IStatementTarget Object { get; set; }
        Result Result { get; set; }
        DateTime? Stored { get; set; }
        DateTime? Timestamp { get; set; }
        Verb Verb { get; set; }
        XAPIVersion Version { get; set; }
    }
}