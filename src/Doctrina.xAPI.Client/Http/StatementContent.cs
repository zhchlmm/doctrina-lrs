using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Client.Http
{
    public class StatementContent : HttpContent
    {
        private readonly Statement _statement;
        //private AttachmentsCollection _attachments;

        public StatementContent(Statement statement)
        {
            _statement = statement;
            base.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypes.Application.Json);
        }

        public void AddAttachment(string contentType, string sha2, byte[] payload)
        {
            base.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypes.Multipart.Mixed);
            // TODO: Added attachment to stream, and change response to multipart/mixed
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            var jsonString = JsonConvert.SerializeObject(_statement, Formatting.None);
            var bytes = Encoding.UTF8.GetBytes(jsonString);

            using (var ms = new MemoryStream(bytes))
            {
                await ms.CopyToAsync(stream);
            }
        }

        protected override bool TryComputeLength(out long length)
        {
            // TODO: TryComputeLength?
            length = 0;
            return false;
        }
    }
}
