using Doctrina.Domain.Entities.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Doctrina.Persistence.Configurations.Documents
{
    public class ActivityProfileConfiguration : IEntityTypeConfiguration<ActivityProfileEntity>
    {
        public void Configure(EntityTypeBuilder<ActivityProfileEntity> builder)
        {
            builder.HasBaseType<DocumentBaseEntity>();

            builder.Property(e => e.ProfileId)
                .IsRequired();

            builder.HasOne(e => e.Activity)
                .WithMany()
                .HasForeignKey(e => e.ActivityHash);

            builder.HasIndex(e => new { e.ProfileId, e.ActivityHash })
               .IsUnique();
        }
    }
}
