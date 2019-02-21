using AutoMapper;
using Doctrina.Domain.Entities;
using Doctrina.Persistence;
using Doctrina.xAPI;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.Agents.Commands
{
    public class MergeActorCommand : IRequest<AgentEntity>
    {
        public MergeActorCommand(Agent agent)
        {
            Member = new HashSet<Agent>();
        }

        public MergeActorCommand()
        {
            Member = new HashSet<Agent>();
        }

        public ObjectType ObjectType { get; set; }

        public string Name { get; set; }

        public Mbox Mbox { get; set; }

        public string MboxSHA1SUM { get; set; }

        public xAPI.Iri OpenId { get; set; }

        public Account Account { get; set; }

        public ICollection<Agent> Member { get; set; }

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
                var entity = ConvertFrom(request);

                // Get from db
                if (!TryGetEntity(entity, out entity))
                {
                    // New agent, or anonomouys group
                    _context.Agents.Add(entity);
                }

                //if (request.ObjectType == xAPI.ObjectType.Agent)
                //{
                //    HandleAgent(entity);
                //}

                if (request.ObjectType == xAPI.ObjectType.Group)
                {
                    HandleGroup(entity);

                    foreach (var member in cmd.Member)
                    {
                        // Ensure Agent exist
                        var grpAgent = MergeActor(member);

                        // Check if the relation exist
                        CreateGroupMemberRelation(entity, grpAgent);
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }


            }

            //private AgentEntity HandleAgent(MergeActorCommand agent)
            //{
            //    //if (agent.IsAnonymous())
            //    //{
            //    //    throw new RequirementException($"An Agent MUST be identified by one (1) of the four types of Inverse Functional Identifiers.");
            //    //}
            //    //if (agent.GetIdentifiers().Count > 1)
            //    //{
            //    //    throw new RequirementException($"An Agent MUST NOT include more than one (1) Inverse Functional Identifier.");
            //    //}

            //    if (TryGetEntity(agent, out AgentEntity entity))
            //    {
            //        return entity;
            //    }

            //    return CreateAgent(agent);
            //}

            //private async Task<AgentEntity> HandleGroup(MergeActorCommand group)
            //{
            //    if (!group.IsAnonymous())
            //    {
            //        if (group.HasMember())
            //        {
            //            CreateGroupMembers(group, entity);
            //        }

            //        return entity;
            //    }
            //    else
            //    {
            //        return HandleAnonymousGroup(group);
            //    }
            //}

            private AgentEntity HandleAnonymousGroup(Group group)
            {
                bool hasMember = group.HasMember();

                //if (group.Member == null)
                //{
                //    throw new RequirementException("An Anonymous Group MUST include a 'member' property listing constituent Agents.");
                //}

                //if (group.Member.Any(x => x.ObjectType == ObjectType.Group))
                //{
                //    throw new RequirementException("An Anonymous Group MUST NOT contain Group Objects in the 'member' identifiers.");
                //}

                var entity = new AgentEntity()
                {
                    ObjectType = EntityObjectType.Group,
                    Name = group.Name
                };

                // A Learning Record Consumer MUST consider each Anonymous Group distinct even if it has an identical set of members.
                _context.Agents.Add(entity);

                if (hasMember)
                {
                    foreach (var member in cmd.Member)
                    {
                        // Ensure Agent exist
                        var grpAgent = MergeActor(member);

                        // Check if the relation exist
                        CreateGroupMemberRelation(entity, grpAgent);
                    }
                }


                return entity;
            }

            private void CreateGroupMembers(Group group, AgentEntity entity)
            {
                if (group.ObjectType != ObjectType.Group)
                    throw new RequirementException("Must have objectType Group");

                foreach (var member in group.Member)
                {
                    // Ensure Agent exist
                    var grpAgent = MergeActor(member);

                    // Check if the relation exist
                    CreateGroupMemberRelation(entity, grpAgent);


                }
            }

            //public AgentEntity MergeActor(Agent actor)
            //{
            //    if (actor == null)
            //        throw new NullReferenceException("actor");

            //    //if (!actor.IsAnonymous() && actor.GetIdentifiers().Count > 1)
            //    //{
            //    //    throw new RequirementException($"An Identified Group/Agent does not allow for multiple identifiers.");
            //    //}

            //    if (actor.ObjectType == ObjectType.Agent)
            //    {
            //        return HandleAgent(actor);
            //    }
            //    else if (actor.ObjectType == ObjectType.Group)
            //    {
            //        return HandleGroup(actor as Group);
            //    }

            //    throw new NotImplementedException();
            //    //throw new RequirementException($"Cannot create Agent/Group with objectType '{actor.ObjectType}'");
            //}

            private bool TryGetEntity(AgentEntity cmd, out AgentEntity entity)
            {
                entity = GetAgentOrGroup(cmd);

                if (entity != null)
                {
                    return true;
                }

                return false;
            }

            public AgentEntity GetAgentOrGroup(AgentEntity cmd)
            {
                var query = _context.Agents.Where(x => x.ObjectType == cmd.ObjectType);

                if (cmd.Mbox != null)
                {
                    return query.FirstOrDefault(x => x.Mbox == cmd.Mbox);
                }

                if (string.IsNullOrEmpty(cmd.Mbox_SHA1SUM))
                {
                    return query.FirstOrDefault(x => x.Mbox_SHA1SUM == cmd.Mbox_SHA1SUM);
                }

                if (cmd.OpenId !=  null)
                {
                    return query.FirstOrDefault(x => x.OpenId == cmd.OpenId);
                }

                if (cmd.Account != null)
                {
                    return query.FirstOrDefault(x => x.Account.HomePage == cmd.Account.HomePage && x.Account.Name == cmd.Account.Name);
                }

                return null;
            }

            public AgentEntity ConvertFrom(MergeActorCommand cmd)
            {
                //if (!(cmd.ObjectType == ObjectType.Agent || cmd.ObjectType == ObjectType.Group))
                //{
                //    throw new RequirementException($"An actor must have objectType Agent or Group.");
                //}

                var entity = new AgentEntity()
                {
                    ObjectType = cmd.ObjectType == ObjectType.Agent ? EntityObjectType.Agent : EntityObjectType.Group,
                    Name = cmd.Name
                };

                if (cmd.Mbox != null)
                {
                    entity.Mbox = cmd.Mbox.ToString();
                }
                else if (!string.IsNullOrWhiteSpace(cmd.MboxSHA1SUM))
                {
                    entity.Mbox_SHA1SUM = cmd.MboxSHA1SUM;
                }
                else if (cmd.OpenId != null)
                {
                    entity.OpenId = cmd.OpenId.ToString();
                }
                else if (cmd.Account != null)
                {
                    entity.Account = new AccountEntity
                    {
                        HomePage = cmd.Account.HomePage.ToString(),
                        Name = cmd.Account.Name
                    };
                }

                if (cmd.Member.Any())
                {
                    CreateGroupMembers(group, entity);
                }

                return entity;
            }

            //public Agent ConvertFrom(AgentEntity entity)
            //{
            //    //if (!(entity.ObjectType == EntityObjectType.Agent || entity.ObjectType == EntityObjectType.Group))
            //    //    throw new RequirementException($"An actor must have objectType Agent or Group.");

            //    Agent agent = new Agent()
            //    {
            //        Name = entity.Name
            //    };

            //    if (entity.ObjectType == EntityObjectType.Group)
            //    {
            //        agent = new Group()
            //        {
            //            Name = entity.Name
            //        };
            //    }

            //    if (entity.Mbox != null)
            //    {
            //        agent.Mbox = new Mbox(entity.Mbox);
            //    }
            //    else if (!string.IsNullOrWhiteSpace(entity.Mbox_SHA1SUM))
            //    {
            //        agent.MboxSHA1SUM = entity.Mbox_SHA1SUM;
            //    }
            //    else if (!string.IsNullOrWhiteSpace(entity.OpenId))
            //    {
            //        agent.OpenId = new xAPI.Iri(entity.OpenId);
            //    }
            //    else if (!string.IsNullOrWhiteSpace(entity.Account.HomePage)
            //      && !string.IsNullOrWhiteSpace(entity.Account.Name))
            //    {
            //        agent.Account = new Account()
            //        {
            //            HomePage = new Uri(entity.Account.HomePage),
            //            Name = entity.Account.Name
            //        };
            //    }

            //    return agent;
            //}
        }
    }
}
