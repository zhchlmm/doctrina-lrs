using System;
using System.IO;
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
        private string _hash;
        private string _contentType;

        private byte[] _content;

        public AttachmentContent(string contentType, byte[] content)
        {
            _contentType = contentType;
            _content = content;
            _hash = ComputeHash(content);
        }

        public AttachmentContent(string contentType, byte[] content, string hash)
        {
            _contentType = contentType;
            _content = content;
            _hash = hash;
        }

        private string ComputeHash(byte[] content)
        {
            var sha = SHA256.Create();
            byte[] hashBytes = sha.ComputeHash(_content);

            return Encoding.UTF8.GetString(hashBytes);
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            var content = new ByteArrayContent(_content);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(_contentType);
            content.Headers.Add("X-Experience-Api-Hash", _hash);
            content.Headers.Add("Content-Transfer-Encoding", "binary");
            await content.CopyToAsync(stream);
        }

        protected override bool TryComputeLength(out long length)
        {
            throw new NotImplementedException();
        }
    }
}
