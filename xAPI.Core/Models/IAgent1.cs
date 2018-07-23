namespace xAPI.Core.Models
{
    public interface IAgent
    {
        Account Account { get; set; }
        Mbox Mbox { get; set; }
        string MboxSHA1SUM { get; set; }
        string Name { get; set; }
        ObjectType ObjectType { get; }
        IRI OpenId { get; set; }
    }
}