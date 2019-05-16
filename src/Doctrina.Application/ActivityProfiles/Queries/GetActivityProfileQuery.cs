using Doctrina.xAPI;
using Doctrina.xAPI.Documents;
using MediatR;
using System;

namespace Doctrina.Application.ActivityProfiles.Queries
{
    public class GetActivityProfileQuery : IRequest<ActivityProfileDocument>
    {
        public string ProfileId { get; set; }
        public Iri ActivityId { get; set; }
        public Guid? Registration { get; set; }
    }
}
