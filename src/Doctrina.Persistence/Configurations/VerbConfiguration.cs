using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations
{
    public class VerbConfiguration : IEntityTypeConfiguration<VerbEntity>
    {
        public void Configure(EntityTypeBuilder<VerbEntity> builder)
        {
            builder.HasKey(e => e.Checksum);
            builder.Property(e => e.Checksum)
                .IsRequired()
                .HasMaxLength(32);

            builder.Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(Constants.MAX_URL_LENGTH);

            builder.Property(e => e.Display)
                .HasConversion<string>();
        }
    }
}
