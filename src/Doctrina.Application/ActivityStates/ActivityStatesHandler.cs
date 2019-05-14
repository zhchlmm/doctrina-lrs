using Doctrina.Application.ActivityStates.Commands;
using Doctrina.xAPI.Documents;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.ActivityStates
{
    public class ActivityStatesHandler :
        IRequestHandler<CreateStateDocumentCommand>,
        IRequestHandler<MergeStateDocumentCommand>,
        IRequestHandler<DeleteActivityStateCommand>,
        IRequestHandler<DeleteActivityStatesCommand>
    {
        public Task<Unit> Handle(CreateStateDocumentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Handle(MergeStateDocumentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Handle(DeleteActivityStateCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Handle(DeleteActivityStatesCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
