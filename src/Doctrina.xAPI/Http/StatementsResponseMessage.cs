using Doctrina.xAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Http
{
    public class StatementsResponse
    {
        public StatementsResponse(HttpContent content)
        {
        }

        public StatementsResult StatementsResult { get; set; }
        public string ContentType { get; set; }
        public List<HttpAttachment> Attachments { get; set; }
    }

    public class HttpAttachment
    {
        public string SHA2 { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}
