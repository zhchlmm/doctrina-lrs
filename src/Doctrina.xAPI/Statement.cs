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
    [JsonConverter(typeof(StatementJsonConverter))]
    [JsonObject()]
    public class Statement : StatementBase
    {
        public Statement() { }
        public Statement(string jsonString) : this((JsonString)jsonString) { }
        public Statement(JsonString jsonString) : this(jsonString.ToJToken()) { }
        public Statement(JToken jtoken) : this(jtoken, ApiVersion.GetLatest()) { }
        public Statement(JToken jtoken, ApiVersion version) : base(jtoken, version)
        {
            if (!AllowObject(jtoken))
            {
                return;
            }

            var jobj = jtoken as JObject;

            if (DisallowNull(jobj["id"]))
            {
                Id = Guid.Parse(jobj.Value<string>("id"));
            }

            if (DisallowNull(jobj["stored"]))
            {
                Stored = DateTimeOffset.Parse(jobj.Value<string>("stored"));
            }

            if (DisallowNull(jobj["authority"]))
            {
                var auth = jtoken.Value<JObject>("authority");
                ObjectType objType = auth.Value<string>("objectType");
                Authority = (Agent)objType.CreateInstance(auth, version);
            }

            DisallowAdditionalProps(jobj, "id", "stored", "authority", "version", "object", "actor", "verb", "result", "context", "timestamp", "attachment");
        }

        /// <summary>
        /// UUID assigned by LRS if not set by the Learning Record Provider.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Timestamp of when this Statement was recorded. Set by LRS.
        /// </summary>
        public DateTimeOffset? Stored { get; set; }

        /// <summary>
        /// Agent or Group who is asserting this Statement is true. 
        /// TODO: Verified by the LRS based on authentication. 
        /// TODO: Set by LRS if not provided or if a strong trust relationship between the Learning Record Provider and LRS has not been established.
        /// </summary>
        public Agent Authority { get; set; }

        /// <summary>
        /// The Statement’s associated xAPI version, formatted according to Semantic Versioning 1.0.0.
        /// </summary>
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

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var obj = base.ToJToken(version, format);
            if (Id != null)
            {
                obj["id"] = Id.ToString();
            }

            if (Stored != null)
            {
                obj["stored"] = Stored;
            }

            if (Authority != null)
            {
                obj["authority"] = Authority.ToJToken(version, format);
            }

            if (Version != null)
            {
                obj["version"] = Version.ToString();
            }

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

        public string ToJson()
        {
            // Override default version
            return base.ToJson(Version, ResultFormat.Exact);
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
