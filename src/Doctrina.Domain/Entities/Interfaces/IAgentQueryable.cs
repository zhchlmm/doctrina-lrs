using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Domain.Entities
{
    public interface IQueryableAgent
    {
        Guid AgentId { get; set; }
        AgentEntity Agent { get; }
    }
}
