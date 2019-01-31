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
                .HasMaxLength(32)
                .IsRequired();

            builder.HasIndex(ac => ac.Id)
                .IsUnique();
            builder.Property(e => e.Id)
                .HasMaxLength(Constants.MAX_URL_LENGTH)
                .IsRequired();

            builder.HasOne(x => x.Definition)
                .WithOne()
                .HasForeignKey<ActivityEntity>(e=> e.DefinitionId);
        }
    }
}
