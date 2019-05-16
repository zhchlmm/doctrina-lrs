using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations
{
    public class ActivityConfiguration : IEntityTypeConfiguration<ActivityEntity>
    {
        public void Configure(EntityTypeBuilder<ActivityEntity> builder)
        {
            builder.HasKey(e => e.ActivityHash);
            builder.Property(e => e.ActivityHash)
                .HasMaxLength(Constants.HASH_LENGTH)
                .IsRequired();

            builder.Property(e => e.ActivityId)
                .HasMaxLength(Constants.MAX_URL_LENGTH)
                .IsRequired();

            builder.HasOne(x => x.Definition)
                .WithOne();
        }
    }
}
