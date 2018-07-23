using Doctrina.Core;
using Doctrina.Core.Persistence.Models;
using System;

namespace Doctrina.Core.Repositories
{
    public class VerbRepository : IVerbRepository
    {
        private readonly DoctrinaDbContext context;

        public VerbRepository(DoctrinaDbContext context)
        {
            this.context = context;
        }

        public void CreateVerb(VerbEntity verb)
        {
            context.Verbs.Add(verb);
        }

        public VerbEntity GetByVerbId(Uri verbId)
        {
            return GetByVerbId(verbId.ToString());
        }

        public VerbEntity GetByVerbId(string verbId)
        {
            return this.context.Verbs.Find(verbId);
        }

        public bool Exist(string verbId)
        {
            return GetByVerbId(verbId) != null;
        }
    }
}
