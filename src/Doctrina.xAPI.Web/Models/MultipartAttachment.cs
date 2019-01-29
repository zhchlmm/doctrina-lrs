using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Doctrina.Web.Models
{
    public class MultipartAttachmentSubtype
    {
        public string ContentType { get; set; }
        public string ContentTransferEncoding { get; set; }
        public byte[] Content { get; set; }
        public string XExperienceApiHash { get; set; }
    }
}
