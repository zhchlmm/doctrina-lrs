using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Web.Areas.xAPI.Authorization
{
    public class ApiScopesFilter : TypeFilterAttribute
    {
        public ApiScopesFilter(Type type) : base(type)
        {
        }
    }
}
