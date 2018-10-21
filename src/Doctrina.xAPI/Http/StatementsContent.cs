using Doctrina.xAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
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
    public class StatementsHttpContent : HttpContent
    {
        private Stream _stream;
        private int multipartIndex = 0;
        private MultipartReader _reader;
        private readonly int _lenghtLimit = 70;
        private MultipartReader MultipartReader
        {
            get
            {
                if(_reader == null)
                {
                    string boundary = GetBoundary();
                    var readStream = this.CreateContentReadStreamAsync().Result;
                    _reader = new MultipartReader(boundary, readStream);
                }
                return _reader;
            }
        }

        private Stream Stream
        {
            get
            {
                if(_stream == null)
                {
                    _stream = CreateContentReadStreamAsync().Result;
                }
                return _stream;
            }
        }

        #region Properties
        /// <summary>
        /// JSON Result (first part of multipart/mixed)
        /// </summary>
        public List<AttachmentBatch> Attachments { get; set; }
        #endregion

        public StatementsHttpContent(string contentType, Stream stream) : base()
        {
            Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
            _stream = stream;
        }

        /// <summary>
        /// Read the first part of the multipart/mixed content, which must be json
        /// </summary>
        /// <returns>JSON string</returns>
        public async Task<string> ReadStatementsString()
        {
            string jsonString = null;
            if (Headers.ContentType.MediaType.StartsWith(MediaTypes.Application.Json))
            {
                using (var streamReader = new System.Net.Http.StreamContent(Stream))
                {
                    jsonString = await streamReader.ReadAsStringAsync();
                }
            }
            else if (Headers.ContentType.MediaType.StartsWith(MediaTypes.Multipart.Mixed))
            {
                var firstPart = await ReadNextAttachmentAsync();
                jsonString = Encoding.UTF8.GetString(firstPart.Content);

                var section = await ReadNextAttachmentAsync();
                if (section != null)
                    Attachments = new List<AttachmentBatch>();

                while(section != null)
                {
                    Attachments.Add(section);
                }
            }

            if (string.IsNullOrEmpty(jsonString))
            {
                throw new NullReferenceException();
            }

            return jsonString;
        }

        public void Add(AttachmentBatch batch)
        {
            if(Headers.ContentType.MediaType != MediaTypes.Multipart.Mixed)
            {
                Headers.ContentType = new MediaTypeHeaderValue(MediaTypes.Multipart.Mixed);
            }

            Attachments.Add(batch);
        }

        public async Task<AttachmentBatch> ReadNextAttachmentAsync()
        {
            var section = await MultipartReader.ReadNextSectionAsync();

            var newAttachment = new AttachmentBatch();

            var contentTypeParameter = MediaTypeHeaderValue.Parse(section.ContentType);
            if(contentTypeParameter != null)
            {
                newAttachment.ContentType = contentTypeParameter;
            }

            if (multipartIndex == 0 && newAttachment.ContentType.MediaType != MediaTypes.Application.Json)
            {
                throw new Exception("For the first part (containing the Statement) this MUST be application/json");
            }

            if(multipartIndex > 0)
            {
                if(section.Headers.TryGetValue("Content-Transfer-Encoding", out StringValues cteValues)){
                    newAttachment.ContentTransferEncoding = MediaTypeHeaderValue.Parse(cteValues);
                }
                else
                {
                    // MUST include a Content-Transfer-Encoding parameter with a value of binary in each part's header after the first (Statements) part.
                    throw new Exception("'Content-Transfer-Encoding' is missing or empty.");
                }

                if (section.Headers.TryGetValue(Constants.Headers.XExperienceApiHash, out StringValues hashValues))
                {
                    newAttachment.XExperienceApiHash = hashValues;
                }
                else
                {
                    // MUST include a Content-Transfer-Encoding parameter with a value of binary in each part's header after the first (Statements) part.
                    throw new Exception($"'{Constants.Headers.XExperienceApiHash}' is missing or empty.");
                }
            }

            using (var ms = new System.IO.MemoryStream())
            {
                section.Body.CopyTo(ms);
                newAttachment.Content = ms.ToArray();
            }

            multipartIndex++;
            return newAttachment;
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

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            // TODO: This read all content as string?
            string jsonString = JsonConvert.SerializeObject(await ReadStatementsString());
            var jsonContent = new StringContent(jsonString, Encoding.UTF8, MediaTypes.Application.Json);

            if(Headers.ContentType.MediaType == MediaTypes.Multipart.Mixed)
            {
                //string boundary = GetBoundary(Headers.ContentType);
                var multipart = new MultipartContent();

                // First part must be application/json
                multipart.Add(jsonContent);

                // Add attachments
                foreach(var attachment in Attachments)
                {
                    string contentType = attachment.ContentType.ToString();
                    var part = new ByteArrayContent(attachment.Content);
                    part.Headers.ContentType = attachment.ContentType;

                    if (attachment.ContentTransferEncoding == null)
                    {
                        throw new NullReferenceException(nameof(attachment.ContentTransferEncoding));

                    }

                    if(attachment.XExperienceApiHash == null)
                    {
                        throw new NullReferenceException(nameof(attachment.XExperienceApiHash));
                    }

                    part.Headers.Add(Constants.Headers.XExperienceApiHash, attachment.XExperienceApiHash);
                    part.Headers.Add(Constants.Headers.ContentTransferEncoding, attachment.ContentTransferEncoding.ToString());

                    multipart.Add(part);
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
        public MediaTypeHeaderValue ContentTransferEncoding { get; set; }
        public byte[] Content { get; set; }
    }
}
