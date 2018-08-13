using Doctrina.Core;
using Doctrina.Core.Data;
using Doctrina.xAPI.Models;
using System;
using System.Linq;

namespace Doctrina.Core.Repositories
{
    public class VerbRepository : IVerbRepository
    {
        private readonly DoctrinaContext context;

        public VerbRepository(DoctrinaContext context)
        {
            this.context = context;
        }

        public void CreateVerb(VerbEntity verb)
        {
            context.Verbs.Add(verb);
        }

        public VerbEntity GetByVerbId(Iri verbId)
        {
            return GetByVerbId(verbId.ToString());
        }

        public VerbEntity GetByVerbId(string verbId)
        {
            return this.context.Verbs.FirstOrDefault(x=> x.Id == verbId);
        }

        public bool Exist(string verbId)
        {
            return GetByVerbId(verbId) != null;
        }
    }
}
