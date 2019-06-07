using Doctrina.xAPI.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Client.Http
{
    public class StatementsResultContent : HttpContent
    {
        private Stream _stream;
        private readonly int _lenghtLimit = 70;
        private Stream ReadStream
        {
            get
            {
                if (_stream == null)
                {
                    _stream = CreateContentReadStreamAsync().Result;
                }
                return _stream;
            }
        }

        public StatementsResult StatementsResultObject { get; private set; }

        #region Properties
        /// <summary>
        /// JSON Result (first part of multipart/mixed)
        /// </summary>
        #endregion

        public StatementsResultContent()
            : base()
        {
        }

        public StatementsResultContent(StatementsResult result)
           : base()
        {
            StatementsResultObject = result;
        }

        public StatementsResultContent(string contentType, Stream stream) : base()
        {
            _stream = stream;
            Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
            string mediaType = Headers.ContentType.MediaType;
            if (!(mediaType == MediaTypes.Application.Json
                || mediaType == MediaTypes.Multipart.Mixed))
            {
                throw new Exception($"Excepted Content-Type of '{MediaTypes.Application.Json}' or '{MediaTypes.Multipart.Mixed}'");
            }
        }

        public async Task<StatementsResult> ReadAsStatementsResultAsync(ApiVersion version)
        {
            if (Headers.ContentType.MediaType.StartsWith(MediaTypes.Application.Json))
            {
                return await ReadAsJsonAsync();
            }
            else if (Headers.ContentType.MediaType.StartsWith(MediaTypes.Multipart.Mixed))
            {
                return await ReadAsMultipartAsync();
            }

            throw new Exception("Content-Type must be Application/Json or Multipart/Mixed");
        }

        public async Task<StatementsResult> ReadAsJsonAsync()
        {
            using (var streamReader = new StreamContent(ReadStream))
            {
                string jsonString = await streamReader.ReadAsStringAsync();
                return new StatementsResult(jsonString);
            }
        }

        public async Task<StatementsResult> ReadAsMultipartAsync()
        {
            var multipartReader = new MultipartReader(GetBoundary(), ReadStream);
            MultipartSection section = await multipartReader.ReadNextSectionAsync();
            int sectionIndex = 0;
            if (section == null)
                return null;

            StatementsResult result = null;

            while (section != null)
            {
                var contentType = MediaTypeHeaderValue.Parse(section.ContentType);

                if (sectionIndex == 0)
                {
                    if (contentType.MediaType != MediaTypes.Application.Json)
                    {
                        throw new MultipartSectionException("For the first part (containing the Statement) this MUST be application/json");
                    }

                    string jsonString = await section.ReadAsStringAsync();
                    result = new StatementsResult(jsonString);
                }
                else
                {
                    var attachmentSection = new MultipartAttachmentSection(section);
                    string hash = attachmentSection.XExperienceApiHash;

                    var attachment = result.GetAttachmentByHash(hash);
                    if (attachment == null)
                    {
                        throw new Exception($"No attachment matched hash '{hash}'");
                    }

                    attachment.SetPayload(await attachmentSection.ReadAsByteArrayAsync());
                }
                // Read next section
                section = await multipartReader.ReadNextSectionAsync();
                sectionIndex++;
            }

            return result;
        }

        /// <summary>
        /// Gets the boundary of the current stream
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private string GetBoundary()
        {
            string boundary = null;
            var boundaryParamter = Headers.ContentType.Parameters.FirstOrDefault(x => x.Name == "boundary");
            if (boundaryParamter != null)
            {
                boundary = boundaryParamter.Value;
            }

            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new UnsupportedContentTypeException("Missing Content-Type boundary.");
            }

            if (boundary.Length > _lenghtLimit)
            {
                throw new UnsupportedContentTypeException($"Multipart boundary length limit {_lenghtLimit} exceeded.");
            }

            return boundary;
        }

        protected override Task<Stream> CreateContentReadStreamAsync()
        {
            return base.CreateContentReadStreamAsync();
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            string jsonString = JsonConvert.SerializeObject(StatementsResultObject);
            var jsonContent = new StringContent(jsonString, Encoding.UTF8, MediaTypes.Application.Json);

            if (Headers.ContentType.MediaType == MediaTypes.Multipart.Mixed)
            {
                //string boundary = GetBoundary(Headers.ContentType);
                var multipart = new MultipartContent
                {
                    // First part must be application/json
                    jsonContent
                };

                var attachments = StatementsResultObject.Statements.SelectMany(x => x.Attachments);

                // Add attachments
                foreach (var attachment in attachments)
                {
                    multipart.Add(new AttachmentContent(attachment));
                }

                await multipart.CopyToAsync(stream, context);
            }
            else
            {
                await jsonContent.CopyToAsync(stream, context);
            }
        }

        protected override bool TryComputeLength(out long length)
        {
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

    public class AttachmentBatch
    {
        public string XExperienceApiHash { get; set; }
        public MediaTypeHeaderValue ContentType { get; set; }
        public TransferCodingHeaderValue ContentTransferEncoding { get; set; }
        public byte[] Content { get; set; }
        public int Length { get; set; }
    }
}
