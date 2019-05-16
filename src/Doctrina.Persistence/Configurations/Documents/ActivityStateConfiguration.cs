using Doctrina.Domain.Entities.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations.Documents
{
    public class ActivityStateConfiguration : IEntityTypeConfiguration<ActivityStateEntity>
    {
        public void Configure(EntityTypeBuilder<ActivityStateEntity> builder)
        {
            builder.Property(e => e.StateId)
                .IsRequired()
                .HasMaxLength(Constants.MAX_URL_LENGTH);

            builder.HasOne(e => e.Activity)
                .WithMany()
                .HasForeignKey(e => e.ActivityHash);

            builder.HasOne(e => e.Agent)
                .WithMany()
                .HasForeignKey(e => e.AgentHash);

            builder.OwnsOne(e => e.Document);

            builder.HasIndex(e => new { e.StateId, e.AgentHash, e.ActivityHash, e.Registration })
                .IsUnique();
        }
    }
}
