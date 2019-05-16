using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Doctrina.xAPI.Http
{
    public class AttachmentContent : ByteArrayContent
    {
        public string XExperienceApiHash
        {
            get
            {
                IEnumerable<string> values;
                if (base.Headers.TryGetValues(Http.Headers.XExperienceApiHash, out values))
                {
                    return values.FirstOrDefault();
                }
                return null;
            }
            set
            {
                base.Headers.TryAddWithoutValidation(Http.Headers.XExperienceApiHash, value);
            }
        }

        //public AttachmentContent()
        //{

        //}

        //public AttachmentContent(string contentType, byte[] content)
        //{
        //    base.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        //    base.Headers.TryAddWithoutValidation(Http.Headers.XExperienceApiHash, ComputeHash(content));
        //    _stream = new MemoryStream(content);
        //}

        //public AttachmentContent(string contentType, byte[] content, string hash)
        //{
        //    base.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        //    base.Headers.TryAddWithoutValidation(Http.Headers.XExperienceApiHash, ComputeHash(content));
        //    _stream = new MemoryStream(content);
        //}

        public AttachmentContent(Attachment attachment)
            : base(attachment.Payload)
        {
            if (string.IsNullOrEmpty(attachment.ContentType))
            {
                throw new ArgumentNullException("Attachment Content-Type must be defined in order to sent payload.");
            }

            if (string.IsNullOrWhiteSpace(attachment.SHA2))
            {
                throw new ArgumentNullException("Attachment SHA-2 must be defined in order to sent payload.");
            }

            if (attachment.Payload == null)
            {
                throw new ArgumentNullException("Attachment Pyload is null.");
            }

            base.Headers.ContentType = new MediaTypeHeaderValue(attachment.ContentType);
            base.Headers.TryAddWithoutValidation(Http.Headers.XExperienceApiHash, attachment.SHA2);
        }
    }
}
