using Doctrina.Core.Persistence.Models;
using Doctrina.Core.Repositories;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Doctrina.xAPI.Models;

namespace Doctrina.Core.Services
{
    public class AgentService : IAgentService
    {
        private readonly DoctrinaDbContext dbContext;
        private readonly AgentGroupMemberRepository groupMembers;
        private readonly IAgentRepository agents;

        public AgentService(DoctrinaDbContext dbContext, IAgentRepository agentRepository)
        {
            this.dbContext = dbContext;
            this.agents = agentRepository;
        }

        //https://github.com/adlnet/ADL_LRS/blob/master/lrs/models.py#L248
        public JObject ToPerson()
        {
            throw new NotImplementedException();
        }

        public AgentEntity MergeAgent(Agent actor)
        {
            if (actor == null)
                throw new NullReferenceException("actor");

            bool isGroup = actor.ObjectType == ObjectType.Group;
            bool isAnonymous = actor.IsAnonymous();

            var entity = GetAgent(actor);

            if (!isAnonymous)
            {
                if (entity == null)
                {
                    entity = CreateAgent(actor);
                }

                if (isGroup && ((Group)actor).HasMember())
                {
                    CreateGroupMembers((Group)actor, entity);
                }

                this.dbContext.SaveChanges();

                return entity;
            }
            else
            {
                return RetreiveOrCreateAnonymousGroup((Group)actor);
            }
            // Only way it doesn't have IFI is if its an anonymous group
            throw new NotSupportedException();
        }

        

        private AgentEntity GetAgent(Agent actor)
        {
            AgentEntity entity = ConvertFrom(actor);
            var match = this.agents.GetAgent(entity);

            if (match != null)
                return match;

            return null;
        }


        private void CreateGroupMembers(Group group, AgentEntity entity)
        {
            if (group.ObjectType != ObjectType.Group)
                throw new Exception("Must have objectType Group");

            foreach (var member in group.Member)
            {
                var grpAgent = MergeAgent(member);

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
            if (group == null || group.Id == null)
                throw new ArgumentNullException("group");

            // New group member must exist
            if (actor == null || actor.Id == null)
                throw new ArgumentNullException("group");

            var related = dbContext.GroupMembers.FirstOrDefault(x=> x.GroupId == group.Id && x.MemberId == actor.Id);

            if (related == null)
            {
                related = new GroupMemberEntity()
                {
                    GroupId = group.Id,
                    MemberId = actor.Id
                };

                this.dbContext.GroupMembers.Add(related);
            }
            return related;
        }

        private AgentEntity RetreiveOrCreateAnonymousGroup(Group group)
        {
            bool hasMember = group.HasMember();

            var entity = new AgentEntity()
            {
                Id = Guid.NewGuid(),
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
        /// Creates agent if it does not exist.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        private AgentEntity CreateAgent(Agent agent)
        {
            AgentEntity entity = ConvertFrom(agent);

            this.agents.Insert(entity);

            this.dbContext.SaveChanges();

            return entity;
        }

        public AgentEntity ConvertFrom(Agent agent)
        {
            if(agent.ObjectType != ObjectType.Agent || agent.ObjectType != ObjectType.Group)
            {
                throw new ArgumentException($"An actor must have objectType Agent or Group.", "agent");
            }

            var entity = new AgentEntity()
            {
                ObjectType = agent.ObjectType == ObjectType.Agent ? EntityObjectType.Agent : EntityObjectType.Group,
                Id = Guid.NewGuid()
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
