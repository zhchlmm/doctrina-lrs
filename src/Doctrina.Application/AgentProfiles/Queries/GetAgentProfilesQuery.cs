using Doctrina.Domain.Entities.Documents;
using Doctrina.xAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Application.AgentProfiles.Queries
{
    public class GetAgentProfilesQuery : IRequest<ICollection<AgentProfileEntity>>
    {
        public Agent Agent { get; set; }
        public DateTimeOffset? Since { get; set; }

        public GetAgentProfilesQuery(Agent agent, DateTimeOffset? since)
        {
            this.Agent = agent;
            this.Since = since;
        }
    }
}
