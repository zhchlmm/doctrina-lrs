using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Core.Data
{
    public class ResultEntity : IResultEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public bool? Success {get;set;}

        public bool? Completion {get;set; }

        public string Response {get;set; }

        public TimeSpan? Duration {get;set; }

        public double? ScoreScaled {get;set; }

        public double? ScoreRaw {get;set;}

        public double? ScoreMin {get;set; }

        public double? ScoreMax {get;set; }

        /// <summary>
        /// Json structure of extensions data
        /// </summary>
        [Column(TypeName = "ntext")]
        public string Extensions {get;set; }
    }
}
