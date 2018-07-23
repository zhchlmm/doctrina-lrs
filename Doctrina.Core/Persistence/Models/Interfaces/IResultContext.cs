using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.LRS.Persistence.Models
{
    public interface IResultEntity
    {
        bool? Success { get; set; }

        bool? Completion { get; set; }

        string Response { get; set; }

        TimeSpan? Duration { get; set; }

        double? ScoreScaled { get; set; }

        double? ScoreRaw { get; set; }

        double? ScoreMin { get; set; }

        double? ScoreMax { get; set; }

        string Extensions { get; set; }
    }
}
