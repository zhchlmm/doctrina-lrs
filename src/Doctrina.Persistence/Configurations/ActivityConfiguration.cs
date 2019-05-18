using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Doctrina.Persistence.Configurations
{
    public class ActivityConfiguration : IEntityTypeConfiguration<ActivityEntity>
    {
        public void Configure(EntityTypeBuilder<ActivityEntity> builder)
        {
            builder.HasKey(x => x.ActivityHash);

            builder.Property(x => x.ActivityHash)
                .HasMaxLength(Constants.HASH_LENGTH)
                .IsRequired();

            builder.Property(e => e.ActivityId)
                .HasMaxLength(Constants.MAX_URL_LENGTH)
                .IsRequired();

            //builder.OwnsOne(x => x.Definition, definitionBuilder => {
            //    definitionBuilder.ToTable("ActivityDefinitions");

            //    definitionBuilder.HasForeignKey("ActivityHash");
            //    definitionBuilder.Property<Guid>("DefinitionId");
            //    definitionBuilder.HasKey("DefinitionId");
            //    definitionBuilder.Property(e => e.Type);

            //    definitionBuilder.Property(e => e.MoreInfo);

            //    definitionBuilder.OwnsMany(p => p.Names, a => {
            //        a.HasForeignKey("ActivityDefinitionId");
            //        a.Property<Guid>("NameId");
            //        a.HasKey("NameId");
            //    });

            //    definitionBuilder.OwnsMany(p => p.Descriptions, a => {
            //        a.HasForeignKey("ActivityDefinitionId");
            //        a.Property<Guid>("DescriptionId");
            //        a.HasKey("DescriptionId");
            //    });

            //    definitionBuilder.OwnsMany(p => p.Extensions, a => {
            //        a.HasForeignKey("ActivityDefinitionId");
            //        a.Property<Guid>("ExtensionId");
            //        a.HasKey("ExtensionId");
            //    });
            //});

            builder.HasOne(x => x.Definition)
                .WithOne(x => x.Activity)
                .HasForeignKey<ActivityDefinitionEntity>(x => x.ActivityHash);
        }
    }
}
