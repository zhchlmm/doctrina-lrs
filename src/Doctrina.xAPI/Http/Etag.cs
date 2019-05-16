namespace Doctrina.xAPI.Http
{
    public class EntityTagHeader
    {
        public string Tag { get; set; }
        public ETagMatch Match { get; set; }
    }
}
