using Doctrina.Persistence.Entities;
using Doctrina.xAPI;
using System.Linq;

namespace Doctrina.Persistence.Repositories
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
