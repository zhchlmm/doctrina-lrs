using Doctrina.Core;
using Doctrina.Core.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Doctrina.Core.Repositories
{
    public class SubStatementRepository : ISubStatementRepository
    {
        private readonly DoctrinaContext _dbContext;

        public SubStatementRepository(DoctrinaContext context)
        {
            this._dbContext = context;
        }

        public void Create(SubStatementEntity subStatement)
        {
            subStatement.Id = Guid.NewGuid();
            _dbContext.SubStatements.Add(subStatement);
            _dbContext.Entry(subStatement).State = EntityState.Added;
        }
    }
}
