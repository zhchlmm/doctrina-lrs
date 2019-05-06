using AutoMapper;
using Doctrina.Application.Agents.Commands;
using Doctrina.Application.Verbs.Commands;
using Doctrina.Domain.Entities;
using Doctrina.Persistence;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.Statements.Commands
{
    public class CreateStatementCommand : IRequest<Guid>
    {
        public Guid? Id { get; set; }

        public xAPI.Verb Verb { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public class Handler : IRequestHandler<CreateStatementCommand, Guid>
        {
            private readonly DoctrinaDbContext _context;
            private readonly IMediator _mediator;
            private readonly IMapper _mapper;

            public Handler(DoctrinaDbContext context, IMediator mediator, IMapper mapper){
                _context = context;
                _mediator = mediator;
                _mapper = mapper;
            }

            public async Task<Guid> Handle(CreateStatementCommand request, CancellationToken cancellationToken)
            {
                // Prevent conflic
                if (request.Id.HasValue)
                {
                    // TODO: Statement Comparision Requirements
                    /// https://github.com/adlnet/xAPI-Spec/blob/master/xAPI-Data.md#statement-comparision-requirements
                    var exist = _context.Statements.Find(request.Id.Value);
                    if (exist != null)
                        return exist.StatementId;
                }

                VerbEntity verb = await _mediator.Send(new MergeVerbCommand() {
                    Id = request.Verb.Id,
                    Display = request.Verb.Display
                });

                
                AgentEntity actor = await _mediator.Send(_mapper.Map<MergeActorCommand>(request.Verb));

                var entity = new StatementEntity()
                {
                    StatementId = request.Id.HasValue ? request.Id.Value : Guid.NewGuid(),
                    ActorId = actor.AgentEntityId,
                    Actor = actor,
                    VerbId = verb.VerbId,
                    Verb = verb,
                    Stored = DateTime.UtcNow,
                    Timestamp = request.Timestamp.Value,
                    // TODO: Implement which store
                };

                _context.Statements.Add(entity);

                return request.Id.Value;
            }
        }
    }
}
