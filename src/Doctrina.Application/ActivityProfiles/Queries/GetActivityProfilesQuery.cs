using Doctrina.Domain.Entities.Documents;
using Doctrina.xAPI;
using MediatR;
using System;
using System.Collections.Generic;

namespace Doctrina.Application.ActivityProfiles.Queries
{
    public class GetActivityProfilesQuery : IRequest<ICollection<ActivityProfileEntity>>
    {
        public Iri ActivityId { get; set; }
        public DateTimeOffset? Since { get; set; }
    }
}
