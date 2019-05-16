using Microsoft.EntityFrameworkCore;

namespace Doctrina.Domain.Entities
{
    [Owned]
    public class Account
    {
        public string HomePage { get; set; }

        public string Name { get; set; }
    }
}
