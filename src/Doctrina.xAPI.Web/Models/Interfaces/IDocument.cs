namespace Doctrina.Persistence.Models
{
    public interface IDocument
    {
        long Id { get; }
        string FileName { get; }
        string ETag { get; }
    }
}