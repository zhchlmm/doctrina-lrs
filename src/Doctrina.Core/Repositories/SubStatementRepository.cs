using Doctrina.Persistence;
using Doctrina.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Doctrina.Persistence.Repositories
{
    public class SubStatementRepository : ISubStatementRepository
    {
        private readonly DoctrinaDbContext _dbContext;

        public SubStatementRepository(DoctrinaDbContext context)
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
