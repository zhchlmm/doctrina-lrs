using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Core.Persistence.Models
{
    public class SubStatementEntity : IStatementObjectEntity, IStatementBase
    {
        [Column(), Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the object attached
        /// </summary>
        [Column, 
            Required]
        public EntityObjectType ObjectType { get; set; }

        [Column()]
        public Guid? ObjectAgentId { get; set; }

        [Column(),
            StringLength(Constants.MAX_URL_LENGTH)]
        public string ObjectActivityId { get; set; }

        [Column()]
        public Guid? ObjectStatementRefId { get; set; }

        [Column()]
        public Guid ActorId { get; set; }

        [Column()]
        public string VerbId { get; set; }

        [Column]
        public Guid ResultId { get; set; }

        [Column()]
        public DateTime? Timestamp { get; set; }

        [Column]
        public Guid ContextId { get; set; }

        #region Navigation Properties
        [ForeignKey(nameof(ActorId))]
        public virtual AgentEntity Actor { get; set; }

        [ForeignKey(nameof(ObjectAgentId))]
        public virtual AgentEntity ObjectAgent { get; set; }

        [ForeignKey(nameof(ObjectActivityId))]
        public virtual ActivityEntity ObjectActivity { get; set; }

        [ForeignKey(nameof(ObjectStatementRefId))]
        public virtual StatementEntity ObjectStatementRef { get; set; }

        [ForeignKey(nameof(VerbId))]
        public virtual VerbEntity Verb { get; set; }

        [ForeignKey(nameof(ResultId))]
        public virtual ResultEntity Result { get; set; }

        [ForeignKey(nameof(ContextId))]
        public virtual ContextEntity Context { get; set; }

        /// <summary>
        /// Gets the attached object
        /// </summary>
        public IStatementObjectEntity Object
        {
            get
            {
                switch (ObjectType)
                {
                    case EntityObjectType.Agent:
                    case EntityObjectType.Group:
                        return ObjectAgent;
                    case EntityObjectType.Activity:
                        return ObjectActivity;
                    case EntityObjectType.StatementRef:
                        return ObjectStatementRef;
                }

                return null;
            }
        }

        #endregion
    }

    //public class GroupMembersRelator
    //{
    //    public Group current;
    //    public Group MapIt(Group group, GroupMember member)
    //    {
    //        // Terminating call.  Since we can return null from this function
    //        // we need to be ready for PetaPoco to callback later with null
    //        // parameters
    //        if (group == null)
    //            return current;

    //        // Is this the same group as the current one we're processing
    //        if (current != null && current.Id == member.GroupId)
    //        {
    //            // Yes, just add this post to the current author's collection of posts
    //            current.GroupMembers.Add(member);

    //            // Return null to indicate we're not done with this author yet
    //            return null;
    //        }

    //        // This is a different author to the current one, or this is the
    //        // first time through and we don't have an author yet

    //        // Save the current author
    //        var prev = current;

    //        // Setup the new current author
    //        current = group;
    //        current.GroupMembers = new List<GroupMember>();
    //        if (member.GroupId == group.Id)
    //        {
    //            current.GroupMembers.Add(member);
    //        }

    //        // Return the now populated previous author (or null if first time through)
    //        return prev;
    //    }
    //}
}
