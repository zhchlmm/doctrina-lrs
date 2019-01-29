using Doctrina.Persistence.Entities;
using Doctrina.xAPI;

namespace Doctrina.Persistence.Services
{
    public interface IAgentService
    {
        AgentEntity MergeActor(Agent actor);
        AgentEntity ConvertFrom(Agent actor);
        Person GetPerson(Agent agent);
    }
}
