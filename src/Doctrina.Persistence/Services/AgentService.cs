using Doctrina.Domain.Entities;
using Doctrina.Persistence.Entities;
using Doctrina.xAPI;
using Doctrina.xAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Doctrina.Persistence.Services
{
    public class AgentService : IAgentService
    {
        private readonly DoctrinaDbContext _dbContext;

        public AgentService(DoctrinaDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Gets the person object for an agent
        /// </summary>
        /// <param name="agent">Agent to match</param>
        /// <returns>Person object</returns>
        public Person GetPerson(Agent agent)
        {
            var person = new Person();
            person.Add(agent);

            if (TryGetEntity(agent, out AgentEntity entity))
            {
                person.Add(ConvertFrom(entity));
            }

            return person;
        }

        /// <summary>
        /// Merge an actor without persisting
        /// </summary>
        /// <param name="actor">Actor to merge</param>
        /// <returns></returns>
        public AgentEntity MergeActor(Agent actor)
        {
            if (actor == null)
                throw new NullReferenceException("actor");

            if(!actor.IsAnonymous() && actor.GetIdentifiers().Count > 1)
            {
                throw new RequirementException($"An Identified Group/Agent does not allow for multiple identifiers.");
            }

            if (actor.ObjectType == ObjectType.Agent)
            {
                return HandleAgent(actor);
            }else if(actor.ObjectType == ObjectType.Group)
            {
                return HandleGroup(actor as Group);
            }

            throw new RequirementException($"Cannot create Agent/Group with objectType '{actor.ObjectType}'");
        }

        private AgentEntity HandleGroup(Group group)
        {
            AgentEntity entity = null;

            if(!TryGetEntity(group, out entity))
            {
                entity = CreateAgent(group);
            }

            if (!group.IsAnonymous())
            {
                if (group.HasMember())
                {
                    CreateGroupMembers(group, entity);
                }

                return entity;
            }
            else
            {
                return HandleAnonymousGroup(group);
            }
        }

        private AgentEntity HandleAgent(Agent agent)
        {
            if (agent.IsAnonymous())
            {
                throw new RequirementException($"An Agent MUST be identified by one (1) of the four types of Inverse Functional Identifiers.");
            }
            if(agent.GetIdentifiers().Count > 1)
            {
                throw new RequirementException($"An Agent MUST NOT include more than one (1) Inverse Functional Identifier.");
            }

            if (TryGetEntity(agent, out AgentEntity entity))
            {
                return entity;
            }

            return CreateAgent(agent);
        }

        private bool TryGetEntity(Agent actor, out AgentEntity entity)
        {
            entity = null;
            var match = GetAgentOrGroup(ConvertFrom(actor));

            if (match != null)
            {
                entity = match;
                return true;
            }

            return false;
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

        /// <summary>
        /// Creates group and member relation
        /// </summary>
        /// <param name="group"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        private GroupMemberEntity CreateGroupMemberRelation(AgentEntity group, AgentEntity actor)
        {
            if (group.ObjectType != EntityObjectType.Group)
                throw new RequirementException("A group must have objectType Group");

            // At this point, the group must exist
            if (group == null || group.AgentId == null)
                throw new ArgumentNullException("group");

            // New group member must exist
            if (actor == null || actor.AgentId == null)
                throw new ArgumentNullException("group");

            var related = _dbContext.GroupMembers.FirstOrDefault(x=> x.GroupId == group.AgentId && x.MemberId == actor.AgentId);

            if (related == null)
            {
                related = new GroupMemberEntity()
                {
                    GroupId = group.AgentId,
                    MemberId = actor.AgentId
                };

                this._dbContext.GroupMembers.Add(related);
            }
            return related;
        }

        private AgentEntity HandleAnonymousGroup(Group group)
        {
            bool hasMember = group.HasMember();

            if (group.Member == null)
            {
                throw new RequirementException("An Anonymous Group MUST include a 'member' property listing constituent Agents.");
            }

            if(group.Member.Any(x=> x.ObjectType == ObjectType.Group))
            {
                throw new RequirementException("An Anonymous Group MUST NOT contain Group Objects in the 'member' identifiers.");
            }

            var entity = new AgentEntity()
            {
                AgentId = Guid.NewGuid(),
                ObjectType = EntityObjectType.Group,
                Name = group.Name
            };

            // A Learning Record Consumer MUST consider each Anonymous Group distinct even if it has an identical set of members.
            this._dbContext.Agents.Add(entity);

            if (hasMember)
            {
                CreateGroupMembers(group, entity);
            }


            return entity;
        }

        /// <summary>
        /// Creates new agent without persisting
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        private AgentEntity CreateAgent(Agent agent)
        {
            AgentEntity entity = ConvertFrom(agent);

            this._dbContext.Agents.Add(entity);

            return entity;
        }

        /// <summary>
        /// Creates new group, without persisting
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        private AgentEntity CreateGroup(Group group)
        {
            AgentEntity entity = ConvertFrom(group);

            this._dbContext.Agents.Add(entity);

            return entity;
        }

        public AgentEntity GetAgentOrGroup(AgentEntity agent)
        {
            var query = _dbContext.Agents.Where(x => x.ObjectType == agent.ObjectType);

            if (string.IsNullOrEmpty(agent.Mbox))
            {
                return query.FirstOrDefault(x => x.Mbox == agent.Mbox);
            }

            if (string.IsNullOrEmpty(agent.Mbox_SHA1SUM))
            {
                return query.FirstOrDefault(x => x.Mbox_SHA1SUM == agent.Mbox_SHA1SUM);
            }

            if (string.IsNullOrEmpty(agent.OpenId))
            {
                return query.FirstOrDefault(x => x.OpenId == agent.OpenId);
            }

            if (agent.Account != null)
            {
                return query.FirstOrDefault(x => x.Account.HomePage == agent.Account.HomePage && x.Account.Name == agent.Account.Name);
            }
            return null;
        }

        public AgentEntity ConvertFrom(Agent agent)
        {
            //if (!(cmd.ObjectType == ObjectType.Agent || cmd.ObjectType == ObjectType.Group))
            //{
            //    throw new RequirementException($"An actor must have objectType Agent or Group.");
            //}

            var entity = new AgentEntity()
            {
                ObjectType = agent.ObjectType == ObjectType.Agent ? EntityObjectType.Agent : EntityObjectType.Group,
                AgentId = Guid.NewGuid(),
                Name = agent.Name
            };

            if (agent.Mbox != null)
            {
                entity.Mbox = agent.Mbox.ToString();
            }
            else if (!string.IsNullOrWhiteSpace(agent.MboxSHA1SUM))
            {
                entity.Mbox_SHA1SUM = agent.MboxSHA1SUM;
            }
            else if (agent.OpenId != null)
            {
                entity.OpenId = agent.OpenId.ToString();
            }
            else if (agent.Account != null)
            {
                entity.Account = new AccountEntity
                {
                    HomePage = agent.Account.HomePage.ToString(),
                    Name = agent.Account.Name
                };
            }
            else if (agent.ObjectType == ObjectType.Agent)
            {
                // Agents can't be anonymous
                //throw new RequirementException("Cannot create agent without a provided Inverse Functional Indentifier");
            }

            return entity;
        }

        public Agent ConvertFrom(AgentEntity entity)
        {
            //if (!(entity.ObjectType == EntityObjectType.Agent || entity.ObjectType == EntityObjectType.Group))
            //    throw new RequirementException($"An actor must have objectType Agent or Group.");

            Agent agent = new Agent()
            {
                Name = entity.Name
            };

            if (entity.ObjectType == EntityObjectType.Group)
            {
                agent = new Group()
                {
                    Name = entity.Name
                };
            }

            if (entity.Mbox != null)
            {
                agent.Mbox = new Mbox(entity.Mbox);
            }
            else if (!string.IsNullOrWhiteSpace(entity.Mbox_SHA1SUM))
            {
                agent.MboxSHA1SUM = entity.Mbox_SHA1SUM;
            }
            else if (!string.IsNullOrWhiteSpace(entity.OpenId))
            {
                agent.OpenId = new xAPI.Iri(entity.OpenId);
            }
            else if (!string.IsNullOrWhiteSpace(entity.Account.HomePage)
              && !string.IsNullOrWhiteSpace(entity.Account.Name))
            {
                agent.Account = new Account()
                {
                    HomePage = new Uri(entity.Account.HomePage),
                    Name = entity.Account.Name
                };
            }

            return agent;
        }

        #region Events
        //protected virtual void OnCombineInformation(CombineInformationEventArgs e)
        //{
        //    CombineInformation?.Invoke(this, e);
        //}

        /// <summary>
        /// 
        /// </summary>
        //public event EventHandler<CombineInformationEventArgs> CombineInformation;
        #endregion
    }
}
