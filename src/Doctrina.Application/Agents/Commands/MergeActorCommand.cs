using AutoMapper;
using Doctrina.Application.Interfaces.Mapping;
using Doctrina.Domain.Entities;
using Doctrina.Persistence;
using Doctrina.xAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.Agents.Commands
{
    public class MergeActorCommand : IRequest<AgentEntity>
    {
        public AgentEntity Actor { get; private set; }

        public class Handler : IRequestHandler<MergeActorCommand, AgentEntity>
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

            public async Task<AgentEntity> Handle(MergeActorCommand request, CancellationToken cancellationToken)
            {
                var agent = request.Actor;
                
                MergeActor(agent);

                return agent;
            }

            /// <summary>
            /// Creates or gets current agent
            /// </summary>
            /// <param name="agent"></param>
            /// <returns></returns>
            private AgentEntity MergeActor(AgentEntity agent)
            {
                // Get from db
                if (TryGetEntity(agent, out AgentEntity currentAgent))
                {
                    //TOOD: Perform agent update logic, add group member etc.
                    HandleMergeActor(agent, currentAgent);

                    return currentAgent;
                }

                // New agent, or anonomouys group
                _context.Agents.Add(agent);

                return agent;
            }

            private void HandleMergeActor(AgentEntity agent, AgentEntity currentAgent)
            {
                throw new NotImplementedException(); 
            }

            private AgentEntity HandleGroup(GroupEntity group)
            {
                CreateGroupMembers(group);

                return group;
            }

            private void CreateGroupMembers(GroupEntity group)
            {
                foreach (var member in group.Members)
                {
                    // Ensure Agent exist
                    var grpAgent = MergeActor(member);

                    // Check if the relation exist
                    CreateGroupMemberRelation(group, grpAgent);
                }
            }

            /// <summary>
            /// Creates group and member relation
            /// </summary>
            /// <param name="group"></param>
            /// <param name="actor"></param>
            /// <returns></returns>
            private void CreateGroupMemberRelation(GroupEntity group, AgentEntity actor)
            {
                var isMember = group.Members.Any(x => x.AgentEntityId == actor.GenerateIdentifierHash());
                if (!isMember)
                {
                    group.Members.Add(actor);
                }
            }

            private bool TryGetEntity(AgentEntity cmd, out AgentEntity entity)
            {
                entity = GetEntity(cmd);

                if (entity != null)
                {
                    return true;
                }

                return false;
            }

            public AgentEntity GetEntity(AgentEntity agentEntity)
            {
                if (string.IsNullOrEmpty(agentEntity.AgentEntityId))
                {
                    agentEntity.AgentEntityId = agentEntity.GenerateIdentifierHash();
                }

                return _context.Agents.FirstOrDefault(x => x.AgentEntityId == agentEntity.AgentEntityId &&
                    x.ObjectType == agentEntity.ObjectType);
            }
        }

        public static MergeActorCommand Create(AgentEntity actor)
        {
            var cmd = new MergeActorCommand()
            {
                Actor = actor
            };

            return cmd;
        }
    }
}
