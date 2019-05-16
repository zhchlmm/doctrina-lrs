using AutoMapper;
using Doctrina.Application.Agents.Commands;
using Doctrina.Domain.Entities;
using Doctrina.Domain.Entities.Documents;
using Doctrina.Persistence;
using Doctrina.xAPI;
using Doctrina.xAPI.Documents;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.ActivityStates.Commands
{
    public class CreateStateDocumentCommand : IRequest<ActivityStateDocument>
    {
        public string StateId { get; set; }
        public Iri ActivityId { get; set; }
        public Agent Agent { get; set; }
        public Guid? Registration { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }

        public class Handler : IRequestHandler<CreateStateDocumentCommand, ActivityStateDocument>
        {
            private readonly DoctrinaDbContext _context;
            private readonly IMediator _mediator;
            private readonly IMapper _mapper;

            public Handler(DoctrinaDbContext context, IMediator mediator, IMapper mapper)
            {
                _context = context;
                _mediator = mediator;
                _mapper = mapper;
            }


            public async Task<ActivityStateDocument> Handle(CreateStateDocumentCommand request, CancellationToken cancellationToken)
            {
                AgentEntity actor = await _mediator.Send(_mapper.Map<MergeActorCommand>(request.Agent));

                var documentEntity = DocumentEntity.Create(request.Content, request.ContentType);

                var activityState = new ActivityStateEntity()
                {
                    StateId = request.StateId,
                    ActivityHash = request.ActivityId.ComputeHash(),
                    Registration = request.Registration,
                    AgentEntityId = actor.AgentHash,
                    Document = documentEntity
                };

                _context.ActivityStates.Add(activityState);
                await _context.SaveChangesAsync();

                return _mapper.Map<ActivityStateDocument>(activityState);
            }
        }
    }
}
