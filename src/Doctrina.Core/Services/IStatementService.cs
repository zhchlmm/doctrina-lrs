using System;
using System.Collections.Generic;
using Doctrina.Core.Persistence.Models;
using Doctrina.xAPI.Models;

namespace Doctrina.Core.Services
{
    public interface IStatementService
    {
        Statement GetStatement(Guid statementId, bool voided = false);
        //IEnumerable<StatementEntity> GetStatements();
        Guid[] SaveStatements(params Statement[] statements);
        Guid SaveStatement(Statement statement);
        bool Exist(Guid statementId, bool voided = false);
    }
}