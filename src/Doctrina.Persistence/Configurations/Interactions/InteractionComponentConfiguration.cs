using Doctrina.Domain.Entities.Interactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Persistence.Configurations.Interactions
{
    public class InteractionComponentConfiguration : IEntityTypeConfiguration<InteractionComponent>
    {
        public void Configure(EntityTypeBuilder<InteractionComponent> builder)
        {
            builder.Property(x => x.Id);
            builder.HasKey(x => x.Id);

            builder.OwnsMany(x => x.Description, description =>
            {
                description.HasForeignKey("ComponentId");
                description.Property<string>("Id");
                description.HasKey("Id");
            });
        }
    }
}
