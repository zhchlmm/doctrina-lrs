using Doctrina.Persistence;
using Doctrina.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Persistence
{
    public class DoctrinaDbContextFactory : DesignTimeDbContextFactoryBase<DoctrinaDbContext>
    {
        protected override DoctrinaDbContext CreateNewInstance(DbContextOptions<DoctrinaDbContext> options)
        {
            return new DoctrinaDbContext(options);
        }
    }
}
