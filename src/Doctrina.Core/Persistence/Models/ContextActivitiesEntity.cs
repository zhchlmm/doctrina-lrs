using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Core.Persistence.Models
{
    public class ContextActivitiesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Key { get; set; }

        public ICollection<ContextActivitiesParent> Parent { get; set; }

        public ICollection<ContextActivitiesGrouping> Grouping { get; set; }

        public ICollection<ContextActivitiesCategory> Category { get; set; }

        public ICollection<ContextActivitiesOther> Other { get; set; }
    }

    public class ContextActivitiesParent
    {
        public Guid ContextId { get; set; }
        public virtual ContextEntity Context { get; set; }

        public string ActivityId { get; set; }
        public virtual ActivityEntity Activity { get; set; }
    }

    public class ContextActivitiesGrouping
    {
        public Guid ContextId { get; set; }
        public virtual ContextEntity Context { get; set; }

        public string ActivityId { get; set; }
        public virtual ActivityEntity Activity { get; set; }
    }

    public class ContextActivitiesCategory
    {
        public Guid ContextId { get; set; }
        public virtual ContextEntity Context { get; set; }

        public string ActivityId { get; set; }
        public virtual ActivityEntity Activity { get; set; }
    }

    public class ContextActivitiesOther
    {
        public Guid ContextId { get; set; }
        public virtual ContextEntity Context { get; set; }

        public string ActivityId { get; set; }
        public virtual ActivityEntity Activity { get; set; }
    }
}
