using Doctrina.Domain.Entities.Documents;
using Doctrina.xAPI;
using MediatR;
using System;

namespace Doctrina.Application.ActivityStates.Commands
{
    public class MergeStateDocumentCommand : IRequest<ActivityStateEntity>
    {
        public string StateId { get; set; }
        public Iri ActivityId { get; set; }
        public Agent Agent { get; set; }
        public Guid? Registration { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}
