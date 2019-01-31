using Doctrina.Domain.Entities;
using Doctrina.xAPI;

namespace Doctrina.Persistence.Repositories
{
    public interface IVerbRepository
    {
        void CreateVerb(VerbEntity verb);
        bool Exist(string verbId);
        VerbEntity GetByVerbId(string verbId);
        VerbEntity GetByVerbId(Iri verbId);
    }
}