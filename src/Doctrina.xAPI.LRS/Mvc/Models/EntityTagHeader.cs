using Doctrina.xAPI.Client.Http;

namespace Doctrina.xAPI.Store.Mvc.Models
{
    public class EntityTagHeader
    {
        public string Tag { get; internal set; }
        public ETagMatch Match { get; internal set; }
    }
}
