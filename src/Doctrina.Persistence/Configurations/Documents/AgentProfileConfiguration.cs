using Doctrina.Domain.Entities.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations.Documents
{
    public class AgentProfileConfiguration : IEntityTypeConfiguration<AgentProfileEntity>
    {
        public void Configure(EntityTypeBuilder<AgentProfileEntity> builder)
        {
            builder.HasKey(e => e.AgentProfileId);
            builder.Property(e => e.AgentProfileId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.ProfileId)
                .HasMaxLength(Constants.MAX_URL_LENGTH)
                .IsRequired();
            builder.HasIndex(e => e.ProfileId)
                .IsUnique();

            builder.Property(e => e.Updated)
                .IsRequired();

            builder.Property(e => e.ContentType)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(e => e.ETag)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(e => e.Agent)
                .WithMany()
                .HasForeignKey(e => e.AgentHash);

            builder.OwnsOne(e => e.Document);
        }
    }
}
