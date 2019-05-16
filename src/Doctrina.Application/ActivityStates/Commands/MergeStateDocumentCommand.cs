using Doctrina.xAPI;
using Doctrina.xAPI.Documents;
using MediatR;
using System;

namespace Doctrina.Application.ActivityStates.Commands
{
    public class MergeStateDocumentCommand : IRequest<ActivityStateDocument>
    {
        public string StateId { get; set; }
        public Iri ActivityId { get; set; }
        public Agent Agent { get; set; }
        public Guid? Registration { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}
