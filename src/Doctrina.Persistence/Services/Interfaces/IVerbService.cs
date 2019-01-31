using Doctrina.Domain.Entities;
using Doctrina.xAPI;

namespace Doctrina.Persistence.Services
{
    public interface IVerbService
    {
        VerbEntity MergeVerb(Verb verb);
    }
}
