using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    [JsonObject]
    public class Result : JsonModel
    {
        public Result() { }
        public Result(JsonString jsonString) : this(jsonString.ToJObject()) { }
        public Result(JObject jobj) : this(jobj, ApiVersion.GetLatest()) { }

        public Result(JObject jobj, ApiVersion version)
        {
            if (jobj["score"] != null)
            {
                Score = new Score(jobj.Value<JObject>("score"), version);
            }
            if (jobj["success"] != null)
            {
                Success = jobj.Value<bool?>("success");
            }
            if (jobj["completion"] != null)
            {
                Completion = jobj.Value<bool?>("completion");
            }

            if (jobj["response"] != null)
            {
                Response = jobj.Value<string>("response");
            }

            if (jobj["duration"] != null)
            {
                Duration = new Duration(jobj.Value<string>("duration"));
            }

            if (jobj["extentions"] != null)
            {
                Extentions = new Extensions(jobj.Value<JObject>("extentions"), version);
            }
        }

        /// <summary>
        /// The score of the Agent in relation to the success or quality of the experience. See: <seealso cref="Models.Score"/>
        /// </summary>
        /// 
        [JsonProperty("score",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public Score Score { get; set; }

        /// <summary>
        /// Indicates whether or not the attempt on the Activity was successful
        /// </summary>
        [JsonProperty("success",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        [JsonConverter(typeof(BooleanConverter))]
        public bool? Success { get; set; }

        /// <summary>
        /// Indicates whether or not the Activity was completed.
        /// </summary>
        [JsonProperty("completion",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        [JsonConverter(typeof(BooleanConverter))]
        public bool? Completion { get; set; }


        /// <summary>
        /// A response appropriately formatted for the given Activity.
        /// </summary>
        [JsonProperty("response",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public string Response { get; set; }

        /// <summary>
        /// Period of time over which the Statement occurred.
        /// </summary>
        [JsonProperty("duration",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public Duration Duration { get; set; }

        /// <summary>
        /// A map of other properties as needed. See: <seealso cref="Models.Extensions"/>
        /// </summary>
        [JsonProperty("extensions",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.Default)]
        public Extensions Extentions { get; set; }

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = new JObject();

            if (Score != null)
            {
                jobj["score"] = Score.ToJToken(version, format);
            }

            if (Success.HasValue)
            {
                jobj["success"] = Success;
            }

            if (Completion.HasValue)
            {
                jobj["completion"] = Completion;
            }

            if (!string.IsNullOrEmpty(Response))
            {
                jobj["response"] = Response;
            }

            if (Duration != null)
            {
                jobj["duration"] = Duration.ToString();
            }

            if (jobj["extentions"] != null)
            {
                Extentions = new Extensions(jobj.Value<JObject>("extentions"), version);
            }

            return jobj;
        }

        public override bool Equals(object obj)
        {
            var result = obj as Result;
            return result != null &&
                   base.Equals(obj) &&
                   EqualityComparer<Score>.Default.Equals(Score, result.Score) &&
                   EqualityComparer<bool?>.Default.Equals(Success, result.Success) &&
                   EqualityComparer<bool?>.Default.Equals(Completion, result.Completion) &&
                   Response == result.Response &&
                   EqualityComparer<Duration>.Default.Equals(Duration, result.Duration) &&
                   EqualityComparer<Extensions>.Default.Equals(Extentions, result.Extentions);
        }

        public override int GetHashCode()
        {
            var hashCode = -1749961971;
            hashCode = hashCode * -1521134295 + EqualityComparer<Score>.Default.GetHashCode(Score);
            hashCode = hashCode * -1521134295 + EqualityComparer<bool?>.Default.GetHashCode(Success);
            hashCode = hashCode * -1521134295 + EqualityComparer<bool?>.Default.GetHashCode(Completion);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Response);
            hashCode = hashCode * -1521134295 + EqualityComparer<Duration>.Default.GetHashCode(Duration);
            hashCode = hashCode * -1521134295 + EqualityComparer<Extensions>.Default.GetHashCode(Extentions);
            return hashCode;
        }

        public static bool operator ==(Result result1, Result result2)
        {
            return EqualityComparer<Result>.Default.Equals(result1, result2);
        }

        public static bool operator !=(Result result1, Result result2)
        {
            return !(result1 == result2);
        }


    }
}
