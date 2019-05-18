using Doctrina.Domain.Entities;
using Doctrina.Domain.Entities.Interactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Doctrina.Persistence.Configurations.Interactions
{
    public class SequencingInteractionTypeConfiguration : IEntityTypeConfiguration<SequencingInteractionType>
    {
        public void Configure(EntityTypeBuilder<SequencingInteractionType> builder)
        {
            builder.HasBaseType<AbstractInteractionType>();

            builder.OwnsMany(x => x.Choices, component => {
                component.ToTable("Sequencing_Choices_Components");
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
