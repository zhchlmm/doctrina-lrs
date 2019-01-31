using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Doctrina.xAPI
{
    public class StatementQuery
    {
        /// <summary>
        /// Id of Statement to fetch (Optional)
        /// </summary>
        [JsonProperty("statementId", NullValueHandling = NullValueHandling.Ignore)]
        public string StatementId { get; set; }

        /// <summary>
        /// Id of voided Statement to fetch. see Voided Statements (Optional)
        /// </summary>
        [JsonProperty("voidedStatementId", NullValueHandling = NullValueHandling.Ignore)]
        public string VoidedStatementId { get; set; }

        /// <summary>
        /// Filter, only return Statements for which the specified Agent or Group is the Actor or Object of the Statement.
        /// <para /> * Agents or Identified Groups are equal when the same Inverse Functional Identifier is used in each Object compared and those Inverse Functional Identifiers have equal values.
        /// <para /> * For the purposes of this filter, Groups that have members which match the specified Agent based on their Inverse Functional Identifier as described above are considered a match
        /// <para />
        /// <para />See agent/group Object definition for details.
        /// </summary>
        [JsonProperty("agent", NullValueHandling = NullValueHandling.Ignore)]
        public object Agent { get; set; }

        /// <summary>
        /// Filter, only return Statements matching the specified Verb id.
        /// </summary>
        [JsonProperty("verb", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Verb { get; set; }

        /// <summary>
        /// Filter, only return Statements for which the Object of the Statement is an Activity with the specified id.
        /// </summary>
        [JsonProperty("activity", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Activity { get; set; }

        /// <summary>
        /// Filter, only return Statements matching the specified registration id. Note that although frequently a unique registration will be used for one Actor assigned to one Activity, this cannot be assumed. If only Statements for a certain Actor or Activity are required, those parameters also need to be specified.
        /// </summary>
        [JsonProperty("registration", NullValueHandling = NullValueHandling.Ignore)]
        public string Registration { get; set; }

        /// <summary>
        /// Apply the Activity filter broadly. Include Statements for which the Object, any of the context Activities, or any of those properties in a contained SubStatement match the Activity parameter, instead of that parameter's normal behavior. Matching is defined in the same way it is for the "activity" parameter.
        /// </summary>
        [JsonProperty("related_activities", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Related_Activities { get; set; }

        /// <summary>
        /// Apply the Agent filter broadly. Include Statements for which the Actor, Object, Authority, Instructor, Team, or any of these properties in a contained SubStatement match the Agent parameter, instead of that parameter's normal behavior. Matching is defined in the same way it is for the "agent" parameter.
        /// </summary>
        [JsonProperty("related_agents", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Related_Agents { get; set; }

        /// <summary>
        /// Only Statements stored since the specified Timestamp (exclusive) are returned.
        /// </summary>
        [JsonProperty("since", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Since { get; set; }

        /// <summary>
        /// Only Statements stored at or before the specified Timestamp are returned.
        /// </summary>
        [JsonProperty("until", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Until { get; set; }

        /// <summary>
        /// Maximum number of Statements to return. 0 indicates return the maximum the server will allow. (Optional)
        /// </summary>
        [JsonProperty("limit", NullValueHandling = NullValueHandling.Ignore)]
        public uint? Limit { get; set; }

        /// <summary>
        /// If ids, only include minimum information necessary in Agent, Activity, Verb and Group Objects to identify them. For Anonymous Groups this means including the minimum information needed to identify each member. 
        /// <para>If exact, return Agent, Activity, Verb and Group Objects populated exactly as they were when the Statement was received.An LRS requesting Statements for the purpose of importing them would use a format of "exact" in order to maintain Statement Immutability.</para>
        /// <para>If canonical, return Activity Objects and Verbs populated with the canonical definition of the Activity Objects and Display of the Verbs as determined by the LRS, after applying the language filtering process defined below, and return the original Agent and Group Objects as in "exact" mode.</para>
        /// </summary>
        [JsonProperty("format")]
        public StatementQueryFormats Format { get; set; } = StatementQueryFormats.exact;

        /// <summary>
        /// If true, the LRS uses the multipart response format and includes all attachments as described previously. 
        /// <para />If false, the LRS sends the prescribed response with Content-Type application/json and does not send attachment data.
        /// </summary>
        [JsonProperty("attachments", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Attachments { get; set; }

        /// <summary>
        /// If true, return results in ascending order of stored time (Newest first)
        /// </summary>
        [JsonProperty("ascending", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Ascending { get; set; }

        public string ToQueryStrings()
        {
            var props = this.GetType().GetProperties();
            var fields = new Dictionary<string, string>();
            //JObject obj = JObject.Parse(JsonConvert.SerializeObject(this));
            //foreach (var token in obj)
            //{
            //    var value = HttpUtility.UrlEncode(obj[token.Key].Value<string>());
            //    fields.Add(token.Key, value);
            //}
            foreach (var prop in props)
            {
                var info = this.GetType().GetProperty(prop.Name);
                if (info == null)
                    continue;

                object retval = info.GetValue(this, null);
                if (retval == null)
                    continue;

                Type type = info.GetType();
                object defaultValue = type.IsValueType ? Activator.CreateInstance(type) : null;
                if (object.Equals(retval, defaultValue))
                    continue;

                fields.Add(prop.Name.ToLowerInvariant(), retval.ToString());
            }

            //TODO: Handle HttpUtillity (HttpUtility.UrlEncode())
            string[] pairs = fields.Select(x => x.Key + "=" + x.Value).ToArray();
            return string.Join("&", pairs);
        }
    }

    public enum StatementQueryFormats
    {
        exact,
        ids,
        canonical
    }
}
