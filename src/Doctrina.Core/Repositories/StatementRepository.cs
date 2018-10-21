using System;
using System.Collections.Generic;
using System.Linq;
using Doctrina.Core.Data;
using Microsoft.EntityFrameworkCore;
using Doctrina.xAPI;

namespace Doctrina.Core.Repositories
{
    public class StatementRepository : IStatementRepository
    {
        private readonly DoctrinaContext dbContext;

        public StatementRepository(DoctrinaContext context)
        {
            this.dbContext = context;
        }

        public StatementEntity GetById(Guid statementId)
        {
            return this.dbContext.Statements.Find(statementId);
        }

        public IQueryable<StatementEntity> GetAll(bool voided, bool includeAttachments)
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
        /// Begins tracking for the StatementEntity
        /// </summary>
        /// <param name="entity"></param>
        public void AddStatement(StatementEntity entity)
        {
            this.dbContext.Statements.Add(entity);
            this.dbContext.Entry(entity).State = EntityState.Added;
            //this.dbContext.SaveChanges();
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
