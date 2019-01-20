namespace Doctrina.xAPI
{
    public interface IAgent
    {
        Account Account { get; set; }
        Mbox Mbox { get; set; }
        string MboxSHA1SUM { get; set; }
        string Name { get; set; }
        ObjectType ObjectType { get; }
        Iri OpenId { get; set; }
    }
}