using Doctrina.xAPI.Client.Http;
using Doctrina.xAPI.Json;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Client
{
    public class JsonModelReader
    {
        public readonly MediaTypeHeaderValue ContentType;
        public readonly Stream Stream;

        public JsonModelReader(MediaTypeHeaderValue contentType, Stream stream)
        {
            ContentType = contentType;
            Stream = stream;
        }

        /// <summary>
        /// Read the stream as TResult
        /// </summary>
        /// <exception cref="JsonModelReaderException"></exception>
        public async Task<TResult> ReadAs<TResult>()
            where TResult : IJsonModel, IAttachmentByHash, new()
        {
            if (ContentType.MediaType == MediaTypes.Application.Json)
            {
                return (TResult)CreateInstance<TResult>(await ReadAsJson(Stream));
            }
            else if (ContentType.MediaType == MediaTypes.Multipart.Mixed)
            {
                return await ReadAsMultipart<TResult>();
            }

            return default(TResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        /// <exception cref="JsonModelReaderException"></exception>
        public async Task<TResult> ReadAsMultipart<TResult>() where TResult : IJsonModel, IAttachmentByHash, new()
        {
            var result = new TResult();

            var boundary = ContentType.Parameters.FirstOrDefault(x => x.Name == "boundary");
            if (boundary == null || string.IsNullOrWhiteSpace(boundary.Value))
                throw new Exception("Content-Type parameter boundary is null or empty.");

            var multipartReader = new MultipartReader(boundary.Value, Stream);
            var section = await multipartReader.ReadNextSectionAsync();
            int sectionIndex = 0;
            while (section != null)
            {
                if (sectionIndex == 0)
                {
                    var sectionContentType = MediaTypeHeaderValue.Parse(section.ContentType);
                    if (sectionContentType.MediaType != MediaTypes.Application.Json)
                    {
                        throw new JsonModelReaderException($"body.form-data[{sectionIndex}]", $"First part must have a Content-Type header value of \"{MediaTypes.Application.Json}\".");
                    }

                    result = (TResult)CreateInstance<TResult>(await ReadAsJson(section.Body));
                }
                else
                {
                    var attachmentSection = new MultipartAttachmentSection(section);
                    string hash = attachmentSection.XExperienceApiHash;
                    var attachment = result.GetAttachmentByHash(hash);
                    if (attachment != null)
                    {
                        attachment.SetPayload(await attachmentSection.ReadAsByteArrayAsync());
                    }
                    else
                    {
                        //result.ParsingErrors.Add($"body.form-data[{sectionIndex}]", $"No attachment match found for '{hash}'");
                        throw new JsonModelReaderException($"body.form-data[{sectionIndex}]", $"Header '{Headers.XExperienceApiHash}: {hash}' does not match any attachments.");
                    }
                }

                section = await multipartReader.ReadNextSectionAsync();
                sectionIndex++;
            }

            return result;
        }

        public async Task<JsonString> ReadAsJson(Stream jsonStream)
        {
            using (StreamReader streamReader = new StreamReader(jsonStream, Encoding.UTF8))
            {
                return await streamReader.ReadToEndAsync();
            }
        }

        private object CreateInstance<TResult>(JsonString jsonString)
            where TResult : IJsonModel, IAttachmentByHash, new()
        {
            var type = typeof(TResult);

            try
            {
                if (type == typeof(Statement))
                {
                    return new Statement(jsonString);
                }
                else if (type == typeof(StatementCollection))
                {
                    return new StatementCollection(jsonString);
                }
                else if (type == typeof(StatementsResult))
                {
                    return new StatementsResult(jsonString);
                }
            }
            catch (JsonModelException ex)
            {
                throw new JsonModelReaderException("", ex.Message);
            }

            return null;
        }
    }
}
