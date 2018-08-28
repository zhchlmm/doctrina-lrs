using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Http
{
    public static class Headers
    {
        public const string ContentTransferEncoding = "Content-Transfer-Encoding";
        public const string ConsistentThrough = "X-Experience-API-Consistent-Through";
        public const string XExperienceApiVersion = "X-Experience-API-Version";
        public const string XExperienceApiHash = "X-Experience-API-Hash";
    }
}
