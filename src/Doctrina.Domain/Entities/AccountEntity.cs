using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Domain.Entities
{
    public class AccountEntity
    {
        public Guid AccountId { get; set; }

        public string HomePage { get; set; }

        public string Name { get; set; }
    }
}
