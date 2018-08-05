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
        void Save(StatementEntity entity);
        StatementEntity GetById(Guid statementRefId);
        void Update(StatementEntity voidedStatement);
        IQueryable<StatementEntity> GetAll(bool voided, bool includeAttachments);
        bool Exist(Guid statementId, bool voided = false);
        bool HasVoidingStatement(Guid id);
        //void VoidStatement(Guid statementId);
        //void VoidStatement(StatementEntity voidedStatement);
    }
}
