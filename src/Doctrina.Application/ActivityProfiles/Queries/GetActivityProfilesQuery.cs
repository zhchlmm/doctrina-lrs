using Doctrina.xAPI;
using Doctrina.xAPI.Documents;
using MediatR;
using System;
using System.Collections.Generic;

namespace Doctrina.Application.ActivityProfiles.Queries
{
    public class GetActivityProfilesQuery : IRequest<ICollection<ActivityProfileDocument>>
    {
        public Iri ActivityId { get; set; }
        public DateTimeOffset? Since { get; set; }
    }
}
