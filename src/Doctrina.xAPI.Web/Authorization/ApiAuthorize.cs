using Microsoft.AspNetCore.Mvc;
using System;

namespace Doctrina.xAPI.LRS.Authorization
{
    public class ApiScopesFilter : TypeFilterAttribute
    {
        public ApiScopesFilter(Type type) : base(type)
        {
        }
    }
}
