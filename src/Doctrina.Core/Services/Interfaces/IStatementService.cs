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
        Guid[] CreateStatements(Agent authority, params Statement[] statements);
        //Guid SaveStatement(Statement statement);
        Statement GetStatement(Guid statementId, bool voided = false, bool inludeAttachments = false);
        IEnumerable<Statement> GetStatements(PagedStatementsQuery parameters, out int totalCount);
        //IEnumerable<StatementEntity> GetStatements();
        bool Exist(Guid statementId, bool voided = false);
    }
}