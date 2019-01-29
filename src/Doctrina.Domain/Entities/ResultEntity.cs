using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Domain.Entities
{
    public class ResultEntity
    {
        public Guid ResultId { get; set; }

        public bool? Success { get; set; }

        public bool? Completion { get; set; }

        public string Response { get; set; }

        /// <summary>
        /// Duration ticks
        /// </summary>
        public long? DurationTicks { get; set; }

        /// <summary>
        /// Duration combination
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// Json structure of extensions data
        /// </summary>
        public DataTypes.ExtensionsDataType Extensions {get;set; }

        public Guid? ScoreId { get; set; }

        #region Navigation
        public virtual StatementEntity Statement { get; set; }
        public virtual ScoreEntity Score { get; set; }
        #endregion
    }
}
