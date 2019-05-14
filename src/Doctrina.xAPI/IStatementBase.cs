using System;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    public interface IStatementBase
    {
        Agent Actor { get; set; }
        ICollection<Attachment> Attachments { get; set; }
        Context Context { get; set; }
        Result Result { get; set; }
        IStatementTarget Object { get; set; }
        DateTimeOffset? Timestamp { get; set; }
        Verb Verb { get; set; }
    }
}