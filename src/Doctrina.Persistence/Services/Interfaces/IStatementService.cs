using Doctrina.Domain.Entities;
using Doctrina.Persistence.Models;
using Doctrina.xAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doctrina.Persistence.Services
{
    public interface IStatementService
    {
        //IEnumerable<StatementEntity> CreateStatements(params Statement[] statements);

        /// <summary>
        /// Create a new statement entity without persisting
        /// </summary>
        /// <param name="statements">Statement object to create as a Statement entity object</param>
        /// <returns>Statement entity object</returns>
        StatementEntity CreateStatement(Statement statements);
        StatementEntity GetStatement(Guid statementId, bool voided = false, bool inludeAttachments = false);
        IEnumerable<StatementEntity> GetStatements(PagedStatementsQuery parameters, out int totalCount);
        DateTimeOffset GetConsistentThroughDate();

        Task SaveAsync(params StatementEntity[] statements);
        void Save(params StatementEntity[] statements);
    }
}