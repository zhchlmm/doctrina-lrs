using Doctrina.Persistence.Entities;

namespace Doctrina.Persistence.Repositories
{
    public interface ISubStatementRepository
    {
        void Create(SubStatementEntity subStatement);
    }
}