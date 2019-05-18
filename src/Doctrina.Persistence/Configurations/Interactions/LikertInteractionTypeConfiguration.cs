using Doctrina.Domain.Entities.Interactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Doctrina.Persistence.Configurations.Interactions
{
    public class LikertInteractionTypeConfiguration : IEntityTypeConfiguration<LikertInteractionType>
    {
        public void Configure(EntityTypeBuilder<LikertInteractionType> builder)
        {
            builder.HasBaseType<AbstractInteractionType>();

            builder.OwnsMany(x => x.Scale, component => {
                component.ToTable("Likert_Scale_Components");
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
