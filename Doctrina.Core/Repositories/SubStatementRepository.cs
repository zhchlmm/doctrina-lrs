using Doctrina.Core;
using Doctrina.Core.Persistence.Models;
using System;

namespace Doctrina.Core.Repositories
{
    public class SubStatementRepository : ISubStatementRepository
    {
        private readonly DoctrinaDbContext context;

        public SubStatementRepository(DoctrinaDbContext context)
        {
            this.context = context;
        }

        public void Create(SubStatementEntity subStatement)
        {
            throw new Exception();
        }
    }
}
