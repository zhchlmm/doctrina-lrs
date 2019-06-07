using Microsoft.AspNetCore.Mvc;
using System;

namespace Doctrina.xAPI.Store.Authorization
{
    public class ApiScopesFilter : TypeFilterAttribute
    {
        public ApiScopesFilter(Type type) : base(type)
        {
        }
    }
}
