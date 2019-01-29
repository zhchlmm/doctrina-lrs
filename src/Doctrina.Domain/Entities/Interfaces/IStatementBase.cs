using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Domain.Entities
{
    public interface IStatementEntityBase
    {
        Guid ActorKey { get; set; }
        Guid VerbKey { get; set; }
        Guid? ObjectAgentKey { get; set; }
        Guid? ObjectActivityKey { get; set; }
        Guid? ObjectStatementRefId { get; set; }

        ContextEntity Context { get; set; }
        ResultEntity Result { get; set; }
        VerbEntity Verb { get; set; }
    }
}
