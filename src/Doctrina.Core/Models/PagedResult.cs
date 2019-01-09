using Doctrina.xAPI;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;

namespace Doctrina.Core.Models
{
    public class PagedStatementsQuery : StatementsQuery
    {
        [FromQuery(Name = "skip")]
        public int? Skip { get; set; }

        [FromHeader(Name = xAPI.Constants.Headers.XExperienceApiVersion)]
        public string Version { get; set; }

        [FromHeader(Name = "Accept-Languge")]
        public string AcceptLanguage { get; set; }

        public override NameValueCollection ToParameterMap(XAPIVersion version)
        {
            var values = base.ToParameterMap(version);
            if (Skip.HasValue)
            {
                values.Add("skip", Skip.Value.ToString());
            }
            return values;
        }
    }
}
