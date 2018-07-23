using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Core.Persistence.Models
{
    public class StatementAttachementEntity
    {
        [Column, Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string CanonicalData { get; set; }

        [Column,
            StringLength(150)]
        public string Payload { get; set; }

        [Column]
        public Guid StatementId { get; set; }

        [ForeignKey(nameof(StatementId))]
        public virtual StatementEntity Statement { get; set; }
    }
}
