using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Doctrina.xAPI.Models;

namespace Doctrina.Core.Persistence.Models
{
    public class VerbEntity
    {
        [Column, Key]
        public Guid Id { get; set; }

        [Column()]
        public string VerbId { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string CanonicalData { get; set; }
    }
}
