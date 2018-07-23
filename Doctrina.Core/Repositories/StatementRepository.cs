using System;
using System.Collections.Generic;
using System.Linq;
using Doctrina.Core.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using xAPI.Core;

namespace Doctrina.Core.Repositories
{
    public class StatementRepository : IStatementRepository
    {
        private readonly DoctrinaDbContext dbContext;

        public StatementRepository(DoctrinaDbContext context)
        {
            this.dbContext = context;
        }

        public StatementEntity GetById(Guid statementId)
        {
            return this.dbContext.Statements.Find(statementId);
        }

        public IQueryable<StatementEntity> GetAll(bool voided = false)
        {
            return this.dbContext.Statements.Where(x => x.Voided == voided);

            //var sql = this.dbContext.Statements

            //    // Actor
            //    .Include(x => x.Actor)

            //    // Verb
            //    .Include(x => x.Verb)

            //    // Authority
            //    .Include(x => x.Authority)

            //    // Object Activity
            //    .Include(x => x.ObjectActivity)

            //    // Object Agent
            //    .Include(x => x.ObjectAgent)

            //    // Object SubStatement
            //    .Include(x => x.ObjectSubStatement);

            //return sql;
        }

        public bool Exist(Guid statementId, bool voided = false)
        {
            var sql = this.dbContext.Statements
                .Where(x => x.StatementId == statementId && x.Voided == voided)
                .Select(x=> new { Id = x.StatementId });

            return sql.Count() == 1;
        }

        public void Save(StatementEntity entity)
        {
            this.dbContext.Statements.Add(entity);
            this.dbContext.Entry(entity).State = EntityState.Added;
            //this.dbContext.SaveChanges();
        }

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
                where stmt.VerbId == Verbs.Voided
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
