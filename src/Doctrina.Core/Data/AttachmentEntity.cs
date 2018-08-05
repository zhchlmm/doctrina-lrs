using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Core.Data
{
    public class AttachmentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string CanonicalData { get; set; }

        [StringLength(255)]
        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        [StringLength(150)]
        public string Payload { get; set; }

        public Guid StatementId { get; set; }


        [ForeignKey(nameof(StatementId))]
        public virtual StatementEntity Statement { get; set; }

    }
}
