using AutoMapper;
using Doctrina.Application.Agents.Commands;
using Doctrina.Application.SubStatements.Commands;
using Doctrina.Application.Verbs.Commands;
using Doctrina.Domain.Entities;
using Doctrina.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.SubStatements
{
    public class SubStatementHandler : IRequestHandler<CreateSubStatementCommand>
    {
        private readonly DoctrinaDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SubStatementHandler(DoctrinaDbContext context, IMediator mediator, IMapper mapper)
        {
            _context = context;
            _mediator = mediator;
            _mapper = mapper;
        }

        public Task<Unit> Handle(CreateSubStatementCommand request, CancellationToken cancellationToken)
        {
            var verb = _mediator.Send(MergeVerbCommand.Create(request.SubStatement.Verb));
            var actor = _mediator.Send(MergeActorCommand.Create(request.SubStatement.Actor));

            var entity = _mapper.Map<SubStatementEntity>(request.SubStatement);
            // TODO: Merge verb
            // TODO: Merge actor


            var SubStatement = new SubStatementEntity()
            {
                Actor = actor,
                Verb = verb,
                Attachments = _mapper.Map<ICollection<AttachmentEntity>>(request.SubStatement.Attachments),
                Result = _mapper.Map<ResultContext>
            }
            
        }
    }
}
