using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI
{
    public interface IObjectType
    {
        ObjectType ObjectType { get; }

        JObject ToJToken(ApiVersion version, ResultFormat format);
    }
}
