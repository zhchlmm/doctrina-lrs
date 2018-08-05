using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI
{
    public static class Constants
    {
        public static class Formats
        {
            public const string DateTimeFormat = "o";
        }

        public static class Headers
        {
            public const string ConsistentThrough = "X-Experience-API-Consistent-Through";
            public const string APIVersion = "X-Experience-API-Version";
            public const string XExperienceAPIHash = "X-Experience-API-Hash";
        }
    }
}
