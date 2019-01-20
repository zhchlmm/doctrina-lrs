using Doctrina.Core.Data;
using Doctrina.xAPI;

namespace Doctrina.Core.Services
{
    public interface IVerbService
    {
        VerbEntity MergeVerb(Verb verb);
    }
}
