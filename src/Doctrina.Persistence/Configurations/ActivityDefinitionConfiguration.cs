using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Doctrina.Persistence.Configurations
{
    public class ActivityDefinitionConfiguration : IEntityTypeConfiguration<ActivityDefinitionEntity>
    {
        public void Configure(EntityTypeBuilder<ActivityDefinitionEntity> builder)
        {
            builder.ToTable("ActivityDefinitions");

            builder.Property(x => x.ActivityDefinitionId)
                .ValueGeneratedOnAdd();
            builder.HasKey(x => x.ActivityDefinitionId);

            builder.Property(e => e.Type);

            builder.Property(e => e.MoreInfo);

            builder.OwnsMany(p => p.Names, a => {
                a.HasForeignKey("ActivityDefinitionId");
                a.Property<Guid>("NameId");
                a.HasKey("NameId");
            });

            builder.OwnsMany(p => p.Descriptions,  a => {
                a.HasForeignKey("ActivityDefinitionId");
                a.Property<Guid>("DescriptionId");
                a.HasKey("DescriptionId");
            });

            builder.OwnsMany(p => p.Extensions, a => { 
                a.HasForeignKey("ActivityDefinitionId");
                a.Property<Guid>("ExtensionId");
                a.HasKey("ExtensionId");
            });
        }
    }
}
