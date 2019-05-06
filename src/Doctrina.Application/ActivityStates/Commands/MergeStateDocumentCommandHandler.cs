using Doctrina.Domain.Entities.Documents;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.ActivityStates.Commands
{
    public class MergeStateDocumentCommandHandler : IRequestHandler<MergeStateDocumentCommand, ActivityStateEntity>
    {
        private readonly IMediator _mediator;

        public MergeStateDocumentCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ActivityStateEntity> Handle(MergeStateDocumentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
