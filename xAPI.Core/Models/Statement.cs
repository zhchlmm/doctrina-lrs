using xAPI.Core.Converters;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace xAPI.Core.Models
{
    /// <summary>
    /// The Statement object
    /// </summary>
    //[JsonConverter(typeof(StatementConverter))]
    [JsonObject]
    public class Statement : StatementBase
    {
        /// <summary>
        /// UUID assigned by LRS if not set by the Learning Record Provider.
        /// </summary>
        [JsonProperty("id",
            Order = 1,
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore)]
        public Guid? Id { get; set; }

        /// <summary>
        /// Timestamp of when this Statement was recorded. Set by LRS.
        /// </summary>
        [JsonProperty("stored",
            Order = 10,
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore)]
        [DataType(DataType.DateTime)]
        public DateTime? Stored { get; set; }

        /// <summary>
        /// Agent or Group who is asserting this Statement is true. 
        /// TODO: Verified by the LRS based on authentication. 
        /// TODO: Set by LRS if not provided or if a strong trust relationship between the Learning Record Provider and LRS has not been established.
        /// </summary>
        [JsonProperty("authority", 
            Order = 11,
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(AgentJsonConverter))]
        public Agent Authority { get; set; }

        /// <summary>
        /// The Statement’s associated xAPI version, formatted according to Semantic Versioning 1.0.0.
        /// </summary>
        [JsonProperty("version",
            Order = 12,
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore)]
        public XAPIVersion Version { get; set; }

        public void Stamp()
        {
            Id = Id.HasValue ? Id : Guid.NewGuid();
            Timestamp = Timestamp.HasValue ? Timestamp : DateTime.UtcNow; 
        }
    }
}
