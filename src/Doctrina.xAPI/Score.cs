using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    [JsonObject]
    [JsonConverter(typeof(ScoreJsonConverter))]
    public class Score : JsonModel
    {
        public Score()
        {
        }

        public Score(string jsonString) : this(JObject.Parse(jsonString))
        {
        }

        public Score(JObject jobj) : this(jobj, ApiVersion.GetLatest())
        {
        }

        public Score(JObject jobj, ApiVersion version)
        {
            if (jobj["scaled"] != null)
            {
                Scaled = jobj.Value<double?>("scaled");
            }

            if (jobj["raw"] != null)
            {
                Raw = jobj.Value<double?>("raw");
            }

            if (jobj["min"] != null)
            {
                Min = jobj.Value<double?>("min");
            }

            if (jobj["max"] != null)
            {
                Max = jobj.Value<double?>("max");
            }
        }

        /// <summary>
        /// The score related to the experience as modified by scaling and/or normalization.
        /// </summary>
        [JsonProperty("scaled",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        [JsonConverter(typeof(NumberConverter))]
        public double? Scaled { get; set; }

        /// <summary>
        /// Decimal number between min and max (if present, otherwise unrestricted), inclusive
        /// The score achieved by the Actor in the experience described by the Statement. This is not modified by any scaling or normalization.
        /// </summary>
        [JsonProperty("raw",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        [JsonConverter(typeof(NumberConverter))]
        public double? Raw { get; set; }

        /// <summary>
        /// Decimal number less than max (if present)
        /// The lowest possible score for the experience described by the Statement.
        /// </summary>
        [JsonProperty("min",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        [JsonConverter(typeof(NumberConverter))]
        public double? Min { get; set; }

        /// <summary>
        /// Decimal number greater than min (if present)
        /// The highest possible score for the experience described by the Statement.
        /// </summary>
        [JsonProperty("max",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        [JsonConverter(typeof(NumberConverter))]
        public double? Max { get; set; }

        public override bool Equals(object obj)
        {
            var score = obj as Score;
            return score != null &&
                   base.Equals(obj) &&
                   EqualityComparer<double?>.Default.Equals(Scaled, score.Scaled) &&
                   EqualityComparer<double?>.Default.Equals(Raw, score.Raw) &&
                   EqualityComparer<double?>.Default.Equals(Min, score.Min) &&
                   EqualityComparer<double?>.Default.Equals(Max, score.Max);
        }

        public override int GetHashCode()
        {
            var hashCode = -1616414752;
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Scaled);
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Raw);
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Min);
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Max);
            return hashCode;
        }

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = new JObject();

            if (Scaled.HasValue)
            {
                jobj["scaled"] = Scaled;
            }

            if (Raw.HasValue)
            {
                jobj["raw"] = Raw;
            }

            if (Min.HasValue)
            {
                jobj["min"] = Min;
            }

            if (Max.HasValue)
            {
                jobj["max"] = Max;
            }

            return jobj;
        }

        public static bool operator ==(Score score1, Score score2)
        {
            return EqualityComparer<Score>.Default.Equals(score1, score2);
        }

        public static bool operator !=(Score score1, Score score2)
        {
            return !(score1 == score2);
        }
    }
}
