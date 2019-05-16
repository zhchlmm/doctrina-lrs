using Doctrina.xAPI;
using Doctrina.xAPI.Documents;
using MediatR;
using System;

namespace Doctrina.Application.ActivityProfiles.Commands
{
    public class CreateActivityProfileCommand : IRequest<ActivityProfileDocument>
    {
        public Iri ActivityId { get; set; }
        public string ProfileId { get; set; }
        public Guid? Registration { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}
