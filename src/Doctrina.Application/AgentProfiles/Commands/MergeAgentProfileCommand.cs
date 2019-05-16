using Doctrina.xAPI;
using Doctrina.xAPI.Documents;
using MediatR;

namespace Doctrina.Application.AgentProfiles.Commands
{
    public class MergeAgentProfileCommand : IRequest<AgentProfileDocument>
    {
        public Agent Agent { get; set; }
        public string ProfileId { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}
