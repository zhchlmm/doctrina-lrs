using Microsoft.EntityFrameworkCore;

namespace Doctrina.Domain.Entities.OwnedTypes
{
    [Owned]
    public class ExtensionEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
