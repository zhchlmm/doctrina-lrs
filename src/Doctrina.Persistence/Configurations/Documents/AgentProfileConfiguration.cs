using Doctrina.Domain.Entities.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations.Documents
{
    public class AgentProfileConfiguration : IEntityTypeConfiguration<AgentProfileEntity>
    {
        public void Configure(EntityTypeBuilder<AgentProfileEntity> builder)
        {
            builder.HasBaseType<DocumentBaseEntity>();

            builder.Property(e => e.ProfileId)
                .HasMaxLength(Constants.MAX_URL_LENGTH)
                .IsRequired();
            builder.HasIndex(e => e.ProfileId)
                .IsUnique();

            builder.HasOne(e => e.Agent)
                .WithMany()
                .HasForeignKey(e => e.AgentHash);

            builder.HasIndex(e => new { e.ProfileId, e.AgentHash })
               .IsUnique();
        }
    }
}
