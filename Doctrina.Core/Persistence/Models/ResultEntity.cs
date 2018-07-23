using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.LRS.Persistence.Models;

namespace Doctrina.Core.Persistence.Models
{
    public class ResultEntity : IResultEntity
    {
        [Column(), Key]
        public Guid Id { get; set; }

        [Column()]
        public bool? Success {get;set;}

        [Column()]
        public bool? Completion {get;set; }

        [Column()]
        public string Response {get;set; }

        [Column()]
        public TimeSpan? Duration {get;set; }

        [Column()]
        public double? ScoreScaled {get;set; }

        [Column()]
        public double? ScoreRaw {get;set;}

        [Column()]
        public double? ScoreMin {get;set; }

        [Column()]
        public double? ScoreMax {get;set; }

        /// <summary>
        /// Json structure of extensions data
        /// </summary>
        [Column(TypeName = "ntext")]
        public string Extensions {get;set; }
    }
}
