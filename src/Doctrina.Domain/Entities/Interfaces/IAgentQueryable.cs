namespace Doctrina.Domain.Entities
{
    public interface IQueryableAgent
    {
        string AgentHash { get; set; }
        AgentEntity Agent { get; }
    }
}
