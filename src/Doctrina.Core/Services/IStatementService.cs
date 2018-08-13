using Doctrina.Core.Models;
using Doctrina.Core.Data;
using Doctrina.xAPI;
using Doctrina.xAPI.Models;
using System;
using System.Collections.Generic;

namespace Doctrina.Core.Services
{
    public interface IStatementService
    {
        Guid[] SaveStatements(Agent authority, params Statement[] statements);
        //Guid SaveStatement(Statement statement);
        Statement GetStatement(Guid statementId, bool voided = false);
        IEnumerable<Statement> GetStatements(PagedStatementsQuery parameters, out int totalCount);
        //IEnumerable<StatementEntity> GetStatements();
        bool Exist(Guid statementId, bool voided = false);
    }
}