using Doctrina.xAPI.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Http
{
    public class MultipartResponseReader
    {
        private readonly int _lenghtLimit = 70;
        private MediaTypeHeaderValue ContentType { get; }
        private Stream Stream { get; }

        public MultipartResponseReader(string contentType, Stream stream)
            : this(MediaTypeHeaderValue.Parse(contentType), stream)
        {
        }

        public MultipartResponseReader(MediaTypeHeaderValue contentType, Stream stream)
        {
            ContentType = contentType;
            Stream = stream;
        }

        public async Task<StatementsResult> ReadAsMultipartAsync()
        {
            var multipartReader = new MultipartReader(GetBoundary(), Stream);
            MultipartSection section = await multipartReader.ReadNextSectionAsync();
            int sectionIndex = 0;
            if (section == null)
                return null;

            StatementsResult result = null;

            try
            {
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
                        result = new StatementsResult((JsonString)jsonString);
                    }
                    else
                    {
                        if (!section.Headers.TryGetValue("Content-Transfer-Encoding", out StringValues cteValues))
                        {
                            string value = cteValues;
                            if (value != "binary")
                            {
                                throw new MultipartSectionException("MUST include a Content-Transfer-Encoding parameter with a value of binary in each part's header after the first (Statements) part.");
                            }
                        }
                        else
                        {
                            // 
                            throw new MultipartSectionException("'Content-Transfer-Encoding' header is missing.");
                        }

                        if (section.Headers.TryGetValue(Headers.XExperienceApiHash, out StringValues hashValues))
                        {
                            string hash = hashValues;
                            var attachment = result.GetAttachmentByHash(hash);
                            if (attachment == null)
                                throw new Exception($"No attachment matched hash '{hash}'");

                            var stringPayload = await section.ReadAsStringAsync();
                            byte[] payloadBytes = Encoding.UTF8.GetBytes(stringPayload);
                            attachment.SetPayload(payloadBytes);
                        }
                        else
                        {
                            // MUST include a Content-Transfer-Encoding parameter with a value of binary in each part's header after the first (Statements) part.
                            throw new MultipartSectionException($"'{Headers.XExperienceApiHash}' is missing.");
                        }
                    }
                    // Read next section
                    section = await multipartReader.ReadNextSectionAsync();
                    sectionIndex++;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new StatementsResultMultipartException($"Multpart section index {sectionIndex}.", ex);
            }
        }

        /// <summary>
        /// Gets the boundary of the current stream
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private string GetBoundary()
        {
            string boundary = null;
            var boundaryParamter = ContentType.Parameters.FirstOrDefault(x => x.Name == "boundary");
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
    }
}
