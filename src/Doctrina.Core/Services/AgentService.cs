using Doctrina.Core.Data;
using Doctrina.Core.Repositories;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Doctrina.xAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Doctrina.Core.Services
{
    public class AgentService : IAgentService
    {
        private readonly DoctrinaContext dbContext;
        private readonly IAgentRepository agents;

        public AgentService(DoctrinaContext dbContext, IAgentRepository agentRepository)
        {
            this.dbContext = dbContext;
            this.agents = agentRepository;
        }

        //https://github.com/adlnet/ADL_LRS/blob/master/lrs/models.py#L248
        public JObject ToPerson()
        {
            throw new NotImplementedException();
        }

        public AgentEntity MergeActor(Agent actor)
        {
            if (actor == null)
                throw new NullReferenceException("actor");

            if(!actor.IsAnonymous() && actor.GetIdentifiers().Count > 1)
            {
                throw new Exception($"An Identified Group/Agent does not allow for multiple identifiers.");
            }

            if (actor.ObjectType == ObjectType.Agent)
            {
                return HandleAgent(actor);
            }else if(actor.ObjectType == ObjectType.Group)
            {
                return HandleGroup(actor as Group);
            }

            throw new Exception($"Cannot create Agent/Group with objectType '{actor.ObjectType}'");
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
                throw new Exception($"An Agent MUST be identified by one (1) of the four types of Inverse Functional Identifiers.");
            }
            if(agent.GetIdentifiers().Count > 1)
            {
                throw new Exception($"An Agent MUST NOT include more than one (1) Inverse Functional Identifier.");
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
            var match = this.agents.GetAgentOrGroup(ConvertFrom(actor));

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
                throw new Exception("Must have objectType Group");

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
                throw new Exception("A group must have objectType Group");

            // At this point, the group must exist
            if (group == null || group.Key == null)
                throw new ArgumentNullException("group");

            // New group member must exist
            if (actor == null || actor.Key == null)
                throw new ArgumentNullException("group");

            var related = dbContext.GroupMembers.FirstOrDefault(x=> x.GroupId == group.Key && x.MemberId == actor.Key);

            if (related == null)
            {
                related = new GroupMemberEntity()
                {
                    GroupId = group.Key,
                    MemberId = actor.Key
                };

                this.dbContext.GroupMembers.Add(related);
            }
            return related;
        }

        private AgentEntity HandleAnonymousGroup(Group group)
        {
            bool hasMember = group.HasMember();

            var entity = new AgentEntity()
            {
                Key = Guid.NewGuid(),
                ObjectType = EntityObjectType.Group,
                Name = group.Name
            };

            // TODO: A Learning Record Consumer MUST consider each Anonymous Group distinct even if it has an identical set of members.
            this.agents.Insert(entity);

            if (hasMember)
            {
                CreateGroupMembers(group, entity);
            }


            return entity;
        }

        /// <summary>
        /// Creates new agent without saving
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        private AgentEntity CreateAgent(Agent agent)
        {
            AgentEntity entity = ConvertFrom(agent);

            this.agents.Insert(entity);
            this.dbContext.Entry(entity).State = EntityState.Added;
            //this.dbContext.SaveChanges();

            return entity;
        }

        /// <summary>
        /// Creates new group, without saving
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        private AgentEntity CreateGroup(Group group)
        {
            AgentEntity entity = ConvertFrom(group);

            this.agents.Insert(entity);
            this.dbContext.Entry(entity).State = EntityState.Added;
            //this.dbContext.SaveChanges();

            return entity;
        }

        public AgentEntity ConvertFrom(Agent agent)
        {
            if(!(agent.ObjectType == ObjectType.Agent || agent.ObjectType == ObjectType.Group))
            {
                throw new ArgumentException($"An actor must have objectType Agent or Group.", "agent");
            }

            var entity = new AgentEntity()
            {
                ObjectType = agent.ObjectType == ObjectType.Agent ? EntityObjectType.Agent : EntityObjectType.Group,
                Key = Guid.NewGuid()
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
                entity.Account_HomePage = agent.Account.HomePage.ToString();
                entity.Account_Name = agent.Account.Name;
            }
            else if(agent.ObjectType == ObjectType.Agent)
            {
                // Agents can't be anonymous
                throw new Exception("Cannot create agent without a provided Inverse Functional Indentifier");
            }

            return entity;
        }

        // TODO: Person API GetCombined
        //public Person GetCombined(Agent agent)
        //{
        //    var person = new Person();
        //    person.Add(agent);
        //    var args = new CombineInformationEventArgs()
        //    {
        //        Agent = agent,
        //        Person = person
        //    };
        //    OnCombineInformation(args);

        //    return args.Person;
        //}

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
