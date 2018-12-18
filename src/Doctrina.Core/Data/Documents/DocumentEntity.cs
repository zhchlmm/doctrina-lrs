using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Core.Data.Documents
{
    public class DocumentEntity : IDocumentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(255)]
        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        [Required]
        [StringLength(50)]
        public string Tag { get; set; }

        [Required]
        public DateTimeOffset LastModified { get; set; }
    }
}
