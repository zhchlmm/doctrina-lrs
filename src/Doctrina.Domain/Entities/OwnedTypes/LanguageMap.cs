using System.Collections.Generic;
using Doctrina.Domain.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Doctrina.Domain.Entities.OwnedTypes
{

    // TODO: Find a way to rollback if statement is voided
    [Owned]
    public class LanguageMapEntity : ValueObject
    {
        public string LanguageCode { get; set; }

        public string Description { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return LanguageCode;
            yield return Description;
        }
    }
}
