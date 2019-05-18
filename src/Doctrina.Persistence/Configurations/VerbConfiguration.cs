using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Doctrina.Persistence.Configurations
{
    public class VerbConfiguration : IEntityTypeConfiguration<VerbEntity>
    {
        public void Configure(EntityTypeBuilder<VerbEntity> builder)
        {
            builder.HasKey(e => e.VerbHash);
            builder.Property(e => e.VerbHash)
                .IsRequired()
                .HasMaxLength(32);

            builder.Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(Constants.MAX_URL_LENGTH);

            builder.OwnsMany(e => e.Display);

            builder.OwnsMany(p => p.Display, a => {
                a.HasForeignKey("VerbHash");
                a.Property<Guid>("DisplayId");
                a.HasKey("DisplayId");
            });
        }
    }
}
