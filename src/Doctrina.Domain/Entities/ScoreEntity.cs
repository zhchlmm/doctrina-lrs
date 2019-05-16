using System;

namespace Doctrina.Domain.Entities
{
    public class ScoreEntity
    {
        public Guid ScoreId { get; set; }

        public double? Scaled { get; set; }

        public double? Raw { get; set; }

        public double? Min { get; set; }

        public double? Max { get; set; }

        public Guid ResultId { get; set; }

        public virtual ResultEntity Result { get; set; }
    }
}
