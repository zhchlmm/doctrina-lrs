using AutoMapper;
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
            private readonly IMapper _mapper;

            public Handler(
                DoctrinaDbContext context,
                IMediator mediator, IMapper mapper)
            {
                _context = context;
                _mediator = mediator;
                _mapper = mapper;
            }

            public Task<Guid[]> Handle(CreateStatementsCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
