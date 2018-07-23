using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Core.Persistence.Models
{
    public interface IStatementBase
    {
        ContextEntity Context { get; set; }
        ResultEntity Result { get; set; }
        Guid ObjectAgentId { get; set; }
        string ObjectActivityId { get; set; }
        Guid? ObjectStatementRefId { get; set; }
        string VerbId { get; set; }
    }
}
