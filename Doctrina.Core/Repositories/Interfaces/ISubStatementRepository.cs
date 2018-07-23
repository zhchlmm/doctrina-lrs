using Doctrina.Core.Persistence.Models;

namespace Doctrina.Core.Repositories
{
    public interface ISubStatementRepository
    {
        void Create(SubStatementEntity subStatement);
    }
}