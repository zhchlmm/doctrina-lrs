using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Http
{
    public class EntityTagHeader
    {
        public string Tag { get; set; }
        public ETagMatch Match { get; set; }
    }
}
