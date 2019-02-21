using Doctrina.xAPI.Helpers;

namespace Doctrina.xAPI.Extensions
{
    public static class IriExtensions
    {
        public static string GenerateSHA1(this Iri iri)
        {
            return SHAHelper.ComputeHash(iri.ToString());
        }
    }
}
