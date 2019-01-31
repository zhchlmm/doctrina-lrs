using Doctrina.Domain.Entities;
using System;
using System.Linq;

namespace Doctrina.Persistence.Repositories
{
    public interface IStatementRepository
    {
        StatementEntity GetById(Guid statementRefId, bool voided, bool includeAttachments);
        void Update(StatementEntity voidedStatement);
        IQueryable<StatementEntity> AsQueryable(bool voided, bool includeAttachments);
        bool HasVoidingStatement(Guid id);
        //void VoidStatement(Guid statementId);
        //void VoidStatement(StatementEntity voidedStatement);
    }
}
