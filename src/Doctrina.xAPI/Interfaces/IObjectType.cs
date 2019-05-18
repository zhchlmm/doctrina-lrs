using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI
{
    public interface IStatementObject
    {
        ObjectType ObjectType { get; }

        JObject ToJToken(ApiVersion version, ResultFormat format);
    }
}
