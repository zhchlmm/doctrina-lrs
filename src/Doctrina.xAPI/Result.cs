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
        public Result(JsonString jsonString) : this(jsonString.ToJToken()) { }
        public Result(JToken jobj) : this(jobj, ApiVersion.GetLatest()) { }

        public Result(JToken token, ApiVersion version)
        {
            if (!AllowObject(token))
            {
                return;
            }

            JObject jobj = token as JObject;

            if (DisallowNull(jobj["score"]))
            {
                Score = new Score(jobj.Value<JObject>("score"), version);
            }
            if (DisallowNull(jobj["success"]) && AllowBoolean(jobj["success"]))
            {
                Success = jobj.Value<bool?>("success");
            }
            if (DisallowNull(jobj["completion"]) && AllowBoolean(jobj["completion"]))
            {
                Completion = jobj.Value<bool?>("completion");
            }

            if (DisallowNull(jobj["response"]))
            {
                Response = jobj.Value<string>("response");
            }

            if (DisallowNull(jobj["duration"]))
            {
                Duration = new Duration(jobj.Value<string>("duration"));
            }

            if (jobj["extensions"] != null)
            {
                Extensions = new Extensions(jobj.Value<JToken>("extensions"), version);
            }

            DisallowAdditionalProps(jobj, "score", "success", "completion", "response", "duration", "extensions");
        }

        /// <summary>
        /// The score of the Agent in relation to the success or quality of the experience. See: <seealso cref="Models.Score"/>
        /// </summary>
        /// 
        public Score Score { get; set; }

        /// <summary>
        /// Indicates whether or not the attempt on the Activity was successful
        /// </summary>
        public bool? Success { get; set; }

        /// <summary>
        /// Indicates whether or not the Activity was completed.
        /// </summary>
        public bool? Completion { get; set; }


        /// <summary>
        /// A response appropriately formatted for the given Activity.
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Period of time over which the Statement occurred.
        /// </summary>
        public Duration Duration { get; set; }

        /// <summary>
        /// A map of other properties as needed. See: <seealso cref="Models.Extensions"/>
        /// </summary>
        public Extensions Extensions { get; set; }

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

            if (jobj["extensions"] != null)
            {
                Extensions = new Extensions(jobj.Value<JObject>("extenions"), version);
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
                   EqualityComparer<Extensions>.Default.Equals(Extensions, result.Extensions);
        }

        public override int GetHashCode()
        {
            var hashCode = -1749961971;
            hashCode = hashCode * -1521134295 + EqualityComparer<Score>.Default.GetHashCode(Score);
            hashCode = hashCode * -1521134295 + EqualityComparer<bool?>.Default.GetHashCode(Success);
            hashCode = hashCode * -1521134295 + EqualityComparer<bool?>.Default.GetHashCode(Completion);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Response);
            hashCode = hashCode * -1521134295 + EqualityComparer<Duration>.Default.GetHashCode(Duration);
            hashCode = hashCode * -1521134295 + EqualityComparer<Extensions>.Default.GetHashCode(Extensions);
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
