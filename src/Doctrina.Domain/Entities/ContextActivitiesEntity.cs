using System;
using System.Collections.Generic;

namespace Doctrina.Domain.Entities
{
    public class ContextActivitiesEntity
    {
        public ContextActivitiesEntity()
        {
            Parent = new HashSet<ContextActivityTypeEntity>();

            Grouping = new HashSet<ContextActivityTypeEntity>();

            Category = new HashSet<ContextActivityTypeEntity>();

            Other = new HashSet<ContextActivityTypeEntity>();
        }

        public Guid ContextActivitiesId { get; set; }

        public ICollection<ContextActivityTypeEntity> Parent { get; set; }

        public ICollection<ContextActivityTypeEntity> Grouping { get; set; }

        public ICollection<ContextActivityTypeEntity> Category { get; set; }

        public ICollection<ContextActivityTypeEntity> Other { get; set; }
    }

    public class ContextActivityTypeEntity
    {
        /// <summary>
        /// Activity IRL ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Activity IRL SHA-1 hash
        /// </summary>
        public string ActivityId { get; set; }
    }
}
