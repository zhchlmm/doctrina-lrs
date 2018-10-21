using Doctrina.xAPI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Core.Models
{
    public class PagedStatementsQuery : StatementsQuery
    {
        [FromQuery(Name = "skip")]
        public int? Skip { get; set; }

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
