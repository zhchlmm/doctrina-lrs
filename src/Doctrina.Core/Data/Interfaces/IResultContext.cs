namespace Doctrina.Core.Data
{
    public interface IResultEntity
    {
        bool? Success { get; set; }

        bool? Completion { get; set; }

        string Response { get; set; }

        long? Duration { get; set; }

        double? ScoreScaled { get; set; }

        double? ScoreRaw { get; set; }

        double? ScoreMin { get; set; }

        double? ScoreMax { get; set; }

        string Extensions { get; set; }
    }
}
