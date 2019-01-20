using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doctrina.Core.Data;

namespace Doctrina.Core.Repositories
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
