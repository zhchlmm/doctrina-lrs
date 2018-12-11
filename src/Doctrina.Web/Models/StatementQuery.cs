using Doctrina.xAPI;
using Microsoft.AspNetCore.Mvc;

namespace Doctrina.Web.Models
{
    public class StatementQuery : xAPI.StatementsQuery
    {
        [FromHeader(Name = Constants.Headers.XExperienceApiVersion)]
        public string Version { get; set; }

        [FromHeader(Name = "Accept-Languge")]
        public string AcceptLanguage { get; set; }
    }
}
