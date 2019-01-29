using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Doctrina.xAPI.LRS.Mvc.ValueProviders
{
    public class AgentValueProvider : IValueProvider
    {
        public bool ContainsPrefix(string prefix)
        {
            throw new NotImplementedException();
        }

        public ValueProviderResult GetValue(string key)
        {
            throw new NotImplementedException();
        }
    }
}
