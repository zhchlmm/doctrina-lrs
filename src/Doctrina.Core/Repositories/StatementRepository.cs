using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Doctrina.xAPI;
using Doctrina.Domain.Entities;

namespace Doctrina.Persistence.Repositories
{
    public class StatementRepository : IStatementRepository
    {
        private readonly DoctrinaDbContext dbContext;

        public StatementRepository(DoctrinaDbContext context)
        {
            this.dbContext = context;
        }

        public StatementEntity GetById(Guid statementId, bool voided, bool includeAttachments)
        {
            if (includeAttachments)
            {
                return this.dbContext.Statements
                    .Include(x=> x.Attachments)
                    .FirstOrDefault(x => x.StatementId == statementId && x.Voided == voided);
            }

            return this.dbContext.Statements
                    .FirstOrDefault(x => x.StatementId == statementId && x.Voided == voided);
        }

        public IQueryable<StatementEntity> AsQueryable(bool voided, bool includeAttachments)
        {
            if (includeAttachments)
            {
                return this.dbContext.Statements
                    .Include(x => x.Attachments)
                    .Where(x => x.Voided == voided);
            }

            return this.dbContext.Statements.Where(x => x.Voided == voided);
        }

        public bool Exist(Guid statementId, bool voided = false)
        {
            var sql = this.dbContext.Statements
                .Where(x => x.StatementId == statementId && x.Voided == voided)
                .Select(x=> new { Id = x.StatementId });

            return sql.Count() == 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="voidedStatement"></param>
        public void Update(StatementEntity voidedStatement)
        {
            this.dbContext.Statements.Update(voidedStatement);
            this.dbContext.Entry(voidedStatement).State = EntityState.Modified;
            //this.dbContext.SaveChanges();
        }

        //public void VoidStatement(Guid statementId)
        //{
        //    var stmt = this.dbContext.Statements.Find(statementId);
        //    if (stmt != null)
        //    {
        //        stmt.Voided = true;
        //        this.dbContext.Statements.Update(stmt);
        //    }
        //}

        /// <summary>
        /// Gets voiding statement by statement id
        /// </summary>
        /// <param name="voidedStatementId"></param>
        /// <returns></returns>
        public bool HasVoidingStatement(Guid voidedStatementId)
        {
            var voidingStatement = (
                from stmt in this.dbContext.Statements
                where stmt.Verb.Id == Verbs.Voided
                && stmt.ObjectType == EntityObjectType.StatementRef
                && stmt.ObjectSubStatementId == voidedStatementId
                select new {
                    Id = stmt.StatementId
                }
            ).FirstOrDefault();

            return voidingStatement != null;
        }
    }
}
