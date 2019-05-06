using AutoMapper;
using Doctrina.Domain.Entities;
using Doctrina.Persistence;
using Doctrina.xAPI;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.Statements.Queries
{
    public class GetStatementQueryHandler : IRequestHandler<GetStatementQuery, Statement>
    {
        private readonly DoctrinaDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GetStatementQueryHandler(DoctrinaDbContext context, IMediator mediator, IMapper mapper)
        {
            _context = context;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<Statement> Handle(GetStatementQuery request, CancellationToken cancellationToken)
        {
            var entity = new StatementEntity();
            if (request.IncludeAttachments)
            {
                entity = await _context.Statements
                    .Include(x => x.Attachments)
                    .FirstOrDefaultAsync(x => x.StatementId == request.StatementId && x.Voided == false);
            }
            else
            {
                entity = await _context.Statements
                    .FirstOrDefaultAsync(x => x.StatementId == request.StatementId && x.Voided == false);
            }

            if(entity == null)
            {
                return null;
            }

            return Mapper.Map(entity, new Statement());
        }
    }
}
