using AutoMapper;
using Doctrina.Application.AgentProfiles.Commands;
using Doctrina.Application.AgentProfiles.Queries;
using Doctrina.Application.Agents.Commands;
using Doctrina.Domain.Entities;
using Doctrina.Domain.Entities.Documents;
using Doctrina.Domain.Entities.Extensions;
using Doctrina.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.AgentProfiles
{
    public class AgentProfilesHandler :
        IRequestHandler<GetAgentProfilesQuery, ICollection<AgentProfileEntity>>,
        IRequestHandler<GetAgentProfileQuery, AgentProfileEntity>,
        IRequestHandler<DeleteAgentProfileCommand>,
        IRequestHandler<MergeAgentProfileCommand, AgentProfileEntity>,
        IRequestHandler<CreateAgentProfileCommand, AgentProfileEntity>,
        IRequestHandler<UpdateAgentProfileCommand, AgentProfileEntity>
    {
        private readonly DoctrinaDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AgentProfilesHandler(DoctrinaDbContext context, IMediator mediator, IMapper mapper)
        {
            _context = context;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<ICollection<AgentProfileEntity>> Handle(GetAgentProfilesQuery request, CancellationToken cancellationToken)
        {
            var agentEntity = _mapper.Map<AgentEntity>(request.Agent);
            var profiles = _context.AgentProfiles
                .WhereAgent(agentEntity);

            if (request.Since.HasValue)
            {
                profiles.Where(x => x.Document.LastModified >= request.Since.Value);
            }
            profiles.OrderByDescending(x => x.Document.LastModified);

            return await profiles.ToListAsync();
        }

        public async Task<AgentProfileEntity> Handle(GetAgentProfileQuery request, CancellationToken cancellationToken)
        {
            var agentEntity = _mapper.Map<AgentEntity>(request.Agent);
            var profile = await _context.AgentProfiles
                .WhereAgent(agentEntity)
                .SingleOrDefaultAsync(x => x.ProfileId == request.ProfileId);
            return profile;
        }

        public async Task<Unit> Handle(DeleteAgentProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await Handle(GetAgentProfileQuery.Create(request.Agent, request.ProfileId), cancellationToken);

            if (profile != null)
            {
                _context.AgentProfiles.Remove(profile);
            }

            return await Unit.Task;
        }

        public async Task<AgentProfileEntity> Handle(MergeAgentProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await Handle(GetAgentProfileQuery.Create(request.Agent, request.ProfileId), cancellationToken);
            if (profile == null)
            {
                return await Handle(
                    CreateAgentProfileCommand.Create(request.Agent, request.ProfileId, request.Content, request.ContentType),
                    cancellationToken);
            }

            return await Handle(UpdateAgentProfileCommand.Create(request.Agent, request.ProfileId, request.Content, request.ContentType), cancellationToken);
        }

        public async Task<AgentProfileEntity> Handle(CreateAgentProfileCommand request, CancellationToken cancellationToken)
        {
            var mergeActorCommand = _mapper.Map<MergeActorCommand>(request.Agent);
            var agentEntity = await _mediator.Send(mergeActorCommand);
            var profile = new AgentProfileEntity()
            {
                AgentProfileId = Guid.NewGuid(),
                ProfileId = request.ProfileId,
                AgentEntityId = agentEntity.AgentEntityId,
                Document = DocumentEntity.Create(request.Content, request.ContentType)
            };

            _context.AgentProfiles.Add(profile);
            await _context.SaveChangesAsync();
            return profile;
        }

        public async Task<AgentProfileEntity> Handle(UpdateAgentProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await Handle(GetAgentProfileQuery.Create(request.Agent, request.ProfileId), cancellationToken);
            profile.Document.Update(request.Content, request.ContentType);
            profile.Updated = DateTime.UtcNow;
            return profile;
        }
    }
}
