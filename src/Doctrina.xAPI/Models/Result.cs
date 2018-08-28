using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Doctrina.xAPI.Models
{
    [JsonObject]
    public class Result : JsonModel
    {
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

        //public override bool Equals(object obj)
        //{
        //    var result = obj as Result;
        //    return result != null &&
        //           base.Equals(obj) &&
        //           EqualityComparer<Score>.Default.Equals(Score, result.Score) &&
        //           EqualityComparer<bool?>.Default.Equals(Success, result.Success) &&
        //           EqualityComparer<bool?>.Default.Equals(Completion, result.Completion) &&
        //           Response == result.Response &&
        //           EqualityComparer<TimeSpan?>.Default.Equals(Duration, result.Duration) &&
        //           EqualityComparer<Extensions>.Default.Equals(Extentions, result.Extentions);
        //}

        //public override int GetHashCode()
        //{
        //    var hashCode = -1749961971;
        //    hashCode = hashCode * -1521134295 + EqualityComparer<Score>.Default.GetHashCode(Score);
        //    hashCode = hashCode * -1521134295 + EqualityComparer<bool?>.Default.GetHashCode(Success);
        //    hashCode = hashCode * -1521134295 + EqualityComparer<bool?>.Default.GetHashCode(Completion);
        //    hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Response);
        //    hashCode = hashCode * -1521134295 + EqualityComparer<TimeSpan?>.Default.GetHashCode(Duration);
        //    hashCode = hashCode * -1521134295 + EqualityComparer<Extensions>.Default.GetHashCode(Extentions);
        //    return hashCode;
        //}

        //public static bool operator ==(Result left, Result right)
        //{
        //    if (left.Score != right.Score)
        //        return false;

        //    if (left.Success != right.Success)
        //        return false;

        //    if (left.Completion != right.Completion)
        //        return false;

        //    if (left.Response != right.Response)
        //        return false;

        //    if (left.Duration != right.Duration)
        //        return false;

        //    if (left.Extentions != right.Extentions)
        //        return false;

        //    return true;
        //}

        //public static bool operator !=(Result left, Result right)
        //{
        //    return !(left == right);
        //}
    }
}
