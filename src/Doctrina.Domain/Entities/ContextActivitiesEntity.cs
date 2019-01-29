using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Domain.Entities
{
    public class ContextActivitiesEntity
    {
        public Guid ContextActivitiesId { get; set; }

        public ICollection<ContextActivitiesParent> Parent { get; set; }

        public ICollection<ContextActivitiesGrouping> Grouping { get; set; }

        public ICollection<ContextActivitiesCategory> Category { get; set; }

        public ICollection<ContextActivitiesOther> Other { get; set; }
    }

    public class ContextActivitiesParent
    {
        public Guid ContextId { get; set; }

        public string ActivityId { get; set; }
    }

    public class ContextActivitiesGrouping
    {
        public Guid ContextId { get; set; }

        public string ActivityId { get; set; }
    }

    public class ContextActivitiesCategory
    {
        public Guid ContextId { get; set; }

        public string ActivityId { get; set; }
    }

    public class ContextActivitiesOther
    {
        public Guid ContextId { get; set; }

        public string ActivityId { get; set; }
    }
}
