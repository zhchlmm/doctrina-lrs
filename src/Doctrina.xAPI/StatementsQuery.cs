using Doctrina.xAPI.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Doctrina.xAPI
{
    public class StatementsQuery
    {
        public Guid? StatementId { get; set; }
        public Guid? VoidedStatementId { get; set; }

        public Agent Agent { get; set; }
        public Uri VerbId { get; set; }
        private string _activityId;
        public string ActivityId
        {
            get { return _activityId; }
            set
            {
                Uri uri = new Uri(value);
                _activityId = value;
            }
        }
        public Guid? Registration { get; set; }
        public bool? RelatedActivities { get; set; }
        public bool? RelatedAgents { get; set; }
        public DateTime? Since { get; set; }
        public DateTime? Until { get; set; }
        public int? Limit { get; set; }
        public StatementsQueryResultFormat Format { get; set; }
        public bool? Ascending { get; set; }
        public string Language { get; set; }
        public bool? Attachments { get; set; }
        public int? Page { get; set; }

        public StatementsQuery() { }

        public NameValueCollection ToParameterMap(XAPIVersion version)
        {
            var result = HttpUtility.ParseQueryString(String.Empty);

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
                result.Add("activity", ActivityId);
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
            if (Format != null)
            {
                result.Add("format", Format.ToString());
            }
            if (Attachments != null)
            {
                result.Add("attachments", Attachments.Value.ToString());
            }
            if (Ascending != null)
            {
                result.Add("ascending", Ascending.Value.ToString());
            }
            if(Page != null)
            {
                result.Add("page", Page.Value.ToString());
            }

            return result;
        }
    }
}
