namespace Doctrina.xAPI
{
    public interface IInvenseFunctionalIdenfitier
    {
        Account Account { get; set; }
        Mbox Mbox { get; set; }
        string MboxSHA1SUM { get; set; }
        string Name { get; set; }
        Iri OpenId { get; set; }
    }
}
