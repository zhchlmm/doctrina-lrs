namespace Doctrina.xAPI.Documents
{
    public class AgentProfileDocument : Document
    {
        public string ProfileId { get; set; }
        public Agent Agent { get; set; }
    }
}
