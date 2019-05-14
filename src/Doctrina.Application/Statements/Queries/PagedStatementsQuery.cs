using Doctrina.xAPI;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Doctrina.Application.Statements.Queries
{
    public class PagedStatementsQuery : StatementsQuery, IRequest<ICollection<Statement>>
    {
        [FromQuery(Name = "skip")]
        public int? Skip { get; set; }

        [FromHeader(Name = xAPI.Http.Headers.XExperienceApiVersion)]
        public string Version { get; set; }

        [FromHeader(Name = "Accept-Languge")]
        public string AcceptLanguage { get; set; }

        public override NameValueCollection ToParameterMap(ApiVersion version)
        {
            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            var values = base.ToParameterMap(version);
            if (Skip.HasValue)
            {
                values.Add("skip", Skip.Value.ToString());
            }
            return values;
        }
    }
}
