using Doctrina.xAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Specialized;
using System.Web;

namespace Doctrina.xAPI
{
    public class StatementsQuery
    {
        /// <summary>
        /// Id of Statement to fetch	
        /// </summary>
        [FromQuery(Name = "statementId")]
        public Guid? StatementId { get; set; }

        /// <summary>
        /// Id of voided Statement to fetch.
        /// </summary>
        [FromQuery(Name = "voidedStatementId")]
        public Guid? VoidedStatementId { get; set; }

        /// <summary>
        /// Filter, only return Statements for which the specified Agent or Group is the Actor or Object of the Statement.
        /// </summary>
        [FromQuery(Name = "agent")]
        public Agent Agent { get; set; }

        /// <summary>
        /// Filter, only return Statements matching the specified Verb id.	
        /// </summary>
        [FromQuery(Name ="verbId")]
        public Uri VerbId { get; set; }

        /// <summary>
        /// Filter, only return Statements for which the Object of the Statement is an Activity with the specified id.	
        /// </summary>
        [FromQuery(Name ="activityId")]
        public Iri ActivityId { get; set; }

        /// <summary>
        /// Filter, only return Statements matching the specified registration id. Note that although frequently a unique registration will be used for one Actor assigned to one Activity, this cannot be assumed. If only Statements for a certain Actor or Activity are required, those parameters also need to be specified.
        /// </summary>
        [FromQuery(Name = "registration")]
        public Guid? Registration { get; set; }

        [FromQuery(Name = "related_activities")]
        public bool? RelatedActivities { get; set; }

        [FromQuery(Name = "related_agents")]
        public bool? RelatedAgents { get; set; }

        [FromQuery(Name = "since")]
        public DateTime? Since { get; set; }

        [FromQuery(Name = "until")]
        public DateTime? Until { get; set; }

        [FromQuery(Name = "limit")]
        public int? Limit { get; set; }

        [FromQuery(Name = "format")]
        public ResultFormats? Format { get; set; }

        [FromQuery(Name = "ascending")]
        public bool? Ascending { get; set; }

        /// <summary>
        /// If true, the LRS uses the multipart response format and includes all attachments as described previously. If false, the LRS sends the prescribed response with Content-Type application/json and does not send attachment data.	
        /// </summary>
        [FromQuery(Name = "attachments")]
        public bool? Attachments { get; set; }

        public StatementsQuery() { }

        public virtual NameValueCollection ToParameterMap(XAPIVersion version)
        {
            var result = HttpUtility.ParseQueryString(string.Empty);

            if (Agent != null)
            {
                result.Add("agent", Agent.ToJson(version));
            }
            if (VerbId != null)
            {
                result.Add("verb", VerbId.ToString());
            }
            if (ActivityId != null)
            {
                result.Add("activity", ActivityId.ToString());
            }
            if (Registration != null)
            {
                result.Add("registration", Registration.Value.ToString());
            }
            if (RelatedActivities != null)
            {
                result.Add("related_activities", RelatedActivities.Value.ToString());
            }
            if (RelatedAgents != null)
            {
                result.Add("related_agents", RelatedAgents.Value.ToString());
            }
            if (Since != null)
            {
                result.Add("since", Since.Value.ToString(Constants.Formats.DateTimeFormat));
            }
            if (Until != null)
            {
                result.Add("until", Until.Value.ToString(Constants.Formats.DateTimeFormat));
            }
            if (Limit != null)
            {
                result.Add("limit", Limit.ToString());
            }
            if (Format.HasValue)
            {
                result.Add("format", Enum.GetName(typeof(ResultFormats), Format.Value));
            }
            if (Attachments != null)
            {
                result.Add("attachments", Attachments.Value.ToString());
            }
            if (Ascending != null)
            {
                result.Add("ascending", Ascending.Value.ToString());
            }

            return result;
        }
    }
}
