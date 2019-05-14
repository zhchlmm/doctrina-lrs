using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI
{
    public interface IStatementTarget
    {
        ObjectType ObjectType { get; }

        JObject ToJObject(ApiVersion version, ResultFormat format);
    }
}
