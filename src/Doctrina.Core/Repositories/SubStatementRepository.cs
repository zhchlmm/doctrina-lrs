using Doctrina.Core;
using Doctrina.Core.Data;
using System;

namespace Doctrina.Core.Repositories
{
    public class SubStatementRepository : ISubStatementRepository
    {
        private readonly DoctrinaContext context;

        public SubStatementRepository(DoctrinaContext context)
        {
            this.context = context;
        }

        public void Create(SubStatementEntity subStatement)
        {
            throw new Exception();
        }
    }
}
