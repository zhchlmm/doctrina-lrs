using Doctrina.Domain.Entities;
using Doctrina.Persistence;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.Statements.Commands
{
    public class PutStatementCommand : IRequest<Guid>
    {
        public class Handler : IRequestHandler<PutStatementCommand, Guid>
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

            public async Task<Guid> Handle(PutStatementCommand request, CancellationToken cancellationToken)
            {
                var entity = new StatementEntity
                {
                    ObjectObjectType = 
                };

                _context.Statements.Add(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return entity.StatementId;
            }
        }
    }
}
