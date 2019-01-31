using Doctrina.Persistence;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.Statements.Commands
{
    public class CreateStatementsCommand : IRequest<Guid[]>
    {
        public class Handler : IRequestHandler<CreateStatementsCommand, Guid[]>
        {
            private readonly DoctrinaDbContext _context;
            private readonly IMediator _mediator;

            public Handler(
                DoctrinaDbContext context,
                IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }

            public Task<Guid[]> Handle(CreateStatementsCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
