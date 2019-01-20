using Doctrina.Core.Data;
using Doctrina.xAPI;

namespace Doctrina.Core.Services
{
    public interface ISubStatementService
    {
        SubStatementEntity CreateSubStatement(SubStatement subStatement);
    }
}
