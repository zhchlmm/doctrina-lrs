using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI
{
    public interface IStatementObject
    {
        ObjectType ObjectType { get; }

        JToken ToJToken(ApiVersion version, ResultFormat format);
    }
}
