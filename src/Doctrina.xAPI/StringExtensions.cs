using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI
{
    public static class StringExtensions
    {
        public static string EnsureEndsWith(this string str, string endWith)
        {
            if (!str.EndsWith(endWith)) return str + endWith;
            return str;
        }
    }
}
