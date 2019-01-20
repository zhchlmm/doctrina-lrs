using Doctrina.xAPI;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI
{
    [JsonObject]
    public class StatementsResult : IStatementsResult
    {
        public StatementsResult()
        {
        }

        public StatementsResult(string json)
           : this(json, ApiVersion.GetLatest(), ResultFormats.Exact)
        {
        }

        public StatementsResult(string json, ApiVersion version)
            : this(json, version, ResultFormats.Exact)
        {
        }

        public StatementsResult(string json, ApiVersion version, ResultFormats format)
        {
            var apiSerializer = new xAPI.Json.ApiJsonSerializer(version, format);
            var reader = new JsonTextReader(new System.IO.StringReader(json));
            var result = apiSerializer.Deserialize<StatementsResult>(reader);
            Statements = result.Statements;
            More = result.More;
        }


        /// <summary>
        /// List of Statements. If the list returned has been limited (due to pagination), and there are more results, they will be located at the "statements" property within the container located at the IRL provided by the "more" property of this Statement result Object. Where no matching Statements are found, this property will contain an empty array.
        /// </summary>
        [JsonProperty("statements")]
        public Statement[] Statements { get; set; }

        /// <summary>
        /// Relative IRL that can be used to fetch more results, including the full path and optionally a query string but excluding scheme, host, and port. Empty string if there are no more results to fetch.
        /// </summary>
        [JsonProperty("more")]
        public Uri More { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            StringWriter sw = new StringWriter();
            JsonTextWriter writer = new JsonTextWriter(sw);

            // {
            writer.WriteStartObject();

            // "statements": [ ... ]
            writer.WritePropertyName("statements");
            writer.WriteStartArray();
            foreach (var statement in Statements)
            {
                writer.WriteRawValue(statement.ToJson());
            }
            writer.WriteEndArray();

            writer.WritePropertyName("more");
            writer.WriteValue(More);

            // }
            writer.WriteEndObject();

            return sw.ToString();
        }

        public static async Task<StatementsResult> ReadAsMultipartAsync(Stream stream, string boundary, ApiVersion version)
        {
            return await ReadAsMultipartAsync(stream, boundary, version, ResultFormats.Exact);
        }

        public static async Task<StatementsResult> ReadAsMultipartAsync(Stream stream, string boundary, ApiVersion version, ResultFormats format)
        {
            var result = new StatementsResult();

            var multipartReader = new MultipartReader(boundary, stream);
            var section = await multipartReader.ReadNextSectionAsync();
            int sectionIxdex = 0;
            while (section != null)
            {
                if (sectionIxdex == 0)
                {
                    // StatementsResult
                    string jsonString = await section.ReadAsStringAsync();
                    result = new StatementsResult(jsonString, version);
                }
                else
                {
                    
                }

                section = await multipartReader.ReadNextSectionAsync();
                sectionIxdex++;
            }

            return result;
        }

        public Attachment GetAttachmentByHash(string hash)
        {
            throw new NotImplementedException();
        }

        public Attachment GetStatementByHash(string hash)
        {
            throw new NotImplementedException();
        }

        public void SetAttachmentByHash(string hash, byte[] payload)
        {
            throw new NotImplementedException();
        }
    }
}
