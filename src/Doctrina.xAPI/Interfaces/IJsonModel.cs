using Doctrina.xAPI.Collections;

namespace Doctrina.xAPI
{
    public interface IJsonModel
    {
        JsonModelErrorsCollection ParsingErrors { get; }
    }
}