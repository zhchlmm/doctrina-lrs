using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doctrina.Core.Data;
using Doctrina.xAPI.Models;

namespace Doctrina.Core.Services
{
    public interface IAgentService
    {
        AgentEntity MergeActor(Agent actor);
        AgentEntity ConvertFrom(Agent actor);
        Person GetPerson(Agent agent);
    }
}
