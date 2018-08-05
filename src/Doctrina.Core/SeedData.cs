using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Core
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DoctrinaContext>();
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                //context.Users.Add(new Data.DoctrinaUser() { });
            }
        }
    }
}
