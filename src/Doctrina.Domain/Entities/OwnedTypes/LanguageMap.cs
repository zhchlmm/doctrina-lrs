using Microsoft.EntityFrameworkCore;

namespace Doctrina.Domain.Entities.OwnedTypes
{

    // TODO: Find a way to rollback if statement is voided
    [Owned]
    public class LanguageMapEntity
    {
        public string LanguageCode { get; set; }

        public string Description { get; set; }
    }
}
