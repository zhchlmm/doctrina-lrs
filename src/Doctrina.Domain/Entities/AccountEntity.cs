using System;

namespace Doctrina.Domain.Entities
{
    public class AccountEntity
    {
        public Guid AccountId { get; set; }

        public string HomePage { get; set; }

        public string Name { get; set; }
    }
}
