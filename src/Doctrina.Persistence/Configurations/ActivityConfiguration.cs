using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations
{
    public class ActivityConfiguration : IEntityTypeConfiguration<ActivityEntity>
    {
        public void Configure(EntityTypeBuilder<ActivityEntity> builder)
        {
            builder.HasKey(e => e.ActivityId);
            builder.Property(e => e.ActivityId)
                .HasMaxLength(Constants.MAX_URL_LENGTH)
                .IsRequired();

            builder.HasIndex(ac => ac.ActivityEntityId)
                .IsUnique();
            builder.Property(e => e.ActivityEntityId)
                .HasMaxLength(Constants.MD5_LENGTH)
                .IsRequired();

            builder.HasOne(x => x.Definition)
                .WithOne()
                .HasForeignKey<ActivityEntity>(e=> e.DefinitionId);
        }
    }
}
