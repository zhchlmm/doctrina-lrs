using Microsoft.EntityFrameworkCore;
using System;

namespace Doctrina.Domain.Entities
{
    [Owned]
    public class Account
    {
        public string HomePage { get; set; }

        public string Name { get; set; }
    }
}
