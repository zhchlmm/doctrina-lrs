using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Http
{
    public class AttachmentContent : HttpContent
    {
        public string XExperienceApiHash
        {
            get
            {
                IEnumerable<string> values;
                if (base.Headers.TryGetValues(Constants.Headers.XExperienceApiHash, out values))
                {
                    return values.FirstOrDefault();
                }
                return null;
            }
        }
        private MemoryStream _stream;

        public AttachmentContent(string contentType, byte[] content)
        {
            base.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            base.Headers.TryAddWithoutValidation(Constants.Headers.XExperienceApiHash, ComputeHash(content));
            _stream = new MemoryStream(content);
        }

        public AttachmentContent(string contentType, byte[] content, string hash)
        {
            base.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            base.Headers.TryAddWithoutValidation(Constants.Headers.XExperienceApiHash, ComputeHash(content));
            _stream = new MemoryStream(content);
        }

        private string ComputeHash(byte[] content)
        {
            var sha = SHA256.Create();
            byte[] hashBytes = sha.ComputeHash(_stream.ToArray());

            return Encoding.UTF8.GetString(hashBytes);
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            // TODO: Does the headers get streamed too?
            await _stream.CopyToAsync(stream);
        }

        protected override bool TryComputeLength(out long length)
        {
            // TODO: Compute length by serializing to stream.
            if (!_stream.CanSeek)
            {
                length = 0;
                return false;
            }
            else
            {
                length = _stream.Length;
                return true;
            }
        }
    }
}
