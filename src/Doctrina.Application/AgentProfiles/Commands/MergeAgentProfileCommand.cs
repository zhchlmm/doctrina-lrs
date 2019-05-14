using Doctrina.Domain.Entities.Documents;
using Doctrina.xAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Application.AgentProfiles.Commands
{
    public class MergeAgentProfileCommand : IRequest<AgentProfileEntity>
    {
        public Agent Agent { get; set; }
        public string ProfileId { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}
