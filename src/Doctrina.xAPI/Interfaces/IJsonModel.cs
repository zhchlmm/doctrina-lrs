using Doctrina.xAPI.Collections;

namespace Doctrina.xAPI
{
    public interface IJsonModel
    {
        JsonModelFailuresCollection Failures { get; }
    }
}