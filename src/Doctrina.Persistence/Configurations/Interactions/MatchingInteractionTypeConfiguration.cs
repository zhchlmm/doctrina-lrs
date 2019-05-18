using Doctrina.Domain.Entities.Interactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Doctrina.Persistence.Configurations.Interactions
{
    public class MatchingInteractionTypeConfiguration : IEntityTypeConfiguration<MatchingInteractionType>
    {
        public void Configure(EntityTypeBuilder<MatchingInteractionType> builder)
        {
            builder.HasBaseType<AbstractInteractionType>();

            builder.OwnsMany(x => x.Target, component => {
                component.ToTable("Matching_Target_Components");
                component.HasForeignKey("InteractionId");
                component.Property(x => x.Id);
                component.HasKey(x => x.Id);

                component.OwnsMany(x => x.Description, description =>
                {
                    description.HasForeignKey("ComponentId");
                    description.Property<int>("Id");
                    description.HasKey("Id");
                });
            });

            builder.OwnsMany(x => x.Source, component => {
                component.ToTable("Matching_Source_Components");
                component.HasForeignKey("InteractionId");
                component.Property(x => x.Id);
                component.HasKey(x => x.Id);

                component.OwnsMany(x => x.Description, description =>
                {
                    description.HasForeignKey("ComponentId");
                    description.Property<int>("Id");
                    description.HasKey("Id");
                });
            });
        }
    }
}
