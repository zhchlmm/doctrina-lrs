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
            builder.HasKey(e => e.Key);
            builder.Property(e => e.Key)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.ProfileId)
                .IsRequired();

            builder.Property(e => e.UpdateDate)
                .HasDefaultValue(DateTime.UtcNow);

            builder.OwnsOne(e => e.Document);
        }
    }
}
