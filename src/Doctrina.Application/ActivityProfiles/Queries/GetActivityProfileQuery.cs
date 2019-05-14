using Doctrina.Domain.Entities.Documents;
using Doctrina.xAPI;
using MediatR;
using System;

namespace Doctrina.Application.ActivityProfiles.Queries
{
    public class GetActivityProfileQuery : IRequest<ActivityProfileEntity>
    {
        public string ProfileId { get; set; }
        public Iri ActivityId { get; set; }
        public Guid? Registration { get; set; }
    }
}
