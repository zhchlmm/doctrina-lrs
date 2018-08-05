using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Web.Models
{
    public class MultipartAttachment
    {
        public string SHA2 { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }
    }
}
