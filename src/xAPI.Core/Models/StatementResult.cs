using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Core.Models
{
    [JsonObject]
    public class StatementsResult : IStatementsResult
    {
        /// <summary>
        /// List of Statements. If the list returned has been limited (due to pagination), and there are more results, they will be located at the "statements" property within the container located at the IRL provided by the "more" property of this Statement result Object. Where no matching Statements are found, this property will contain an empty array.
        /// </summary>
        [JsonProperty("statements")]
        public IEnumerable<Statement> Statements { get; set; }

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
    }
}
