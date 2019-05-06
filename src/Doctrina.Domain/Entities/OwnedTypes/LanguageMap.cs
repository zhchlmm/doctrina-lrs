using Microsoft.EntityFrameworkCore;

namespace Doctrina.Domain.Entities.OwnedTypes
{
    [Owned]
    public class LanguageMapEntity
    {
        public string LanguageCode { get; set; }

        public string Description { get; set; }
    }
}
