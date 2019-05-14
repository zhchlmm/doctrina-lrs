using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Doctrina.xAPI
{
    /// <summary>
    /// The Statement object
    /// </summary>
    [JsonConverter(typeof(StatementConverter))]
    [JsonObject()]
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
        public DateTimeOffset? Stored { get; set; }

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
        public ApiVersion Version { get; set; }

        public void Stamp()
        {
            Id = Id.HasValue ? Id : Guid.NewGuid();
            Timestamp = Timestamp.HasValue ? Timestamp : DateTime.UtcNow;
        }

        public override bool Equals(object obj)
        {
            var statement = obj as Statement;
            return statement != null &&
                   base.Equals(obj);
                   //&&
                   //EqualityComparer<Guid?>.Default.Equals(Id, statement.Id) &&
                   //EqualityComparer<Agent>.Default.Equals(Authority, statement.Authority) &&
                   //EqualityComparer<XAPIVersion>.Default.Equals(Version, statement.Version);
        }

        public override int GetHashCode()
        {
            var hashCode = -864523893;
            //hashCode = hashCode * -1521134295 + EqualityComparer<Guid?>.Default.GetHashCode(Id);
            //hashCode = hashCode * -1521134295 + EqualityComparer<Agent>.Default.GetHashCode(Authority);
            //hashCode = hashCode * -1521134295 + EqualityComparer<XAPIVersion>.Default.GetHashCode(Version);
            return hashCode;
        }

        public override JObject ToJObject(ApiVersion version, ResultFormat format)
        {
            var obj = base.ToJObject(version, format);
            obj["id"] = Id;
            obj["stored"] = Stored;
            obj["authority"] = Authority.ToJObject(version, format);
            obj["version"] = Version.ToString();
            return obj;
        }

        /// <summary>
        /// Convert the Statement to json using statement version
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Statement as json</returns>
        public override string ToJson(ResultFormat format = ResultFormat.Exact)
        {
            // Override default version
            return base.ToJson(Version, format);
        }

        public static Statement Parse(string jsonString)
        {
            var obj = JObject.Parse(jsonString);

            var statement = new Statement();

            if (obj["id"] != null)
            {
                statement.Id = obj["id"].Value<Guid>();
            }

            if (obj["stored"] != null)
            {
                statement.Stored = obj["stored"].Value<DateTimeOffset?>();
            }

            if(obj["authority"] != null)
            {
                statement.Authority = Agent.Parse(obj["authority"].ToString());
            }

            if(obj["version"] != null)
            {
                statement.Version = obj["version"].Value<string>();
            }

            return statement;
        }

        public static bool operator ==(Statement statement1, Statement statement2)
        {
            return EqualityComparer<Statement>.Default.Equals(statement1, statement2);
        }

        public static bool operator !=(Statement statement1, Statement statement2)
        {
            return !(statement1 == statement2);
        }
    }
}
