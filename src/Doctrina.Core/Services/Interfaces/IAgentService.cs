using Doctrina.Core.Data;
using Doctrina.xAPI;

namespace Doctrina.Core.Services
{
    public interface IAgentService
    {
        AgentEntity MergeActor(Agent actor);
        AgentEntity ConvertFrom(Agent actor);
        Person GetPerson(Agent agent);
    }
}
