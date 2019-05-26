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
            builder.ToTable("Activities");

            builder.Property(e => e.ActivityId)
               .HasMaxLength(Constants.MAX_URL_LENGTH)
               .IsRequired();

            builder.Property(x => x.ActivityHash)
                .HasMaxLength(Constants.HASH_LENGTH)
                .IsRequired();
            builder.HasKey(x => x.ActivityHash);

            builder.HasOne(x => x.Definition);
        }
    }
}
