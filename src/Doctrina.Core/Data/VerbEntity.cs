using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Core.Data
{
    public class VerbEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Key { get; set; }

        public string Id { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string CanonicalData { get; set; }
    }
}
