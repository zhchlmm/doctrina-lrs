using System.Collections.Generic;
using Doctrina.Domain.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Doctrina.Domain.Entities.OwnedTypes
{
    [Owned]
    public class ExtensionEntity : ValueObject
    {
        public string Key { get; set; }
        public string Value { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Key;
            yield return Value;
        }
    }
}
