using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations
{
    public class AgentConfiguration : IEntityTypeConfiguration<AgentEntity>
    {
        public void Configure(EntityTypeBuilder<AgentEntity> builder)
        {
            builder.HasKey(x => x.AgentHash);

            builder.Property(e => e.ObjectType)
                .IsRequired()
                .HasMaxLength(Constants.OBJECT_TYPE_LENGTH);// Or just 5 = Agent or Group

            builder.Property(e => e.Name)
                .HasMaxLength(100);

            builder.Property(e => e.Mbox)
                .HasMaxLength(128);

            builder.Property(e => e.Mbox_SHA1SUM)
                .HasMaxLength(40);

            builder.OwnsOne(e => e.Account);

            builder.HasIndex(x => new { x.ObjectType, x.AgentHash })
                .IsUnique();

            builder
                .HasIndex(x => new { x.ObjectType, x.Mbox })
                .HasFilter("[Mbox] IS NOT NULL")
                .IsUnique();

            builder
                .HasIndex(x => new { x.ObjectType, x.Mbox_SHA1SUM })
                .HasFilter("[Mbox_SHA1SUM] IS NOT NULL")
                .IsUnique();

            builder
                .HasIndex(x => new { x.ObjectType, x.OpenId })
                .HasFilter("[OpenId] IS NOT NULL")
                .IsUnique();

            builder
                .HasIndex(agent => new { agent.ObjectType, agent.Account.HomePage, agent.Account.Name })
                .HasFilter("[Account_HomePage] IS NOT NULL AND [Account_Name] IS NOT NULL")
                .IsUnique();
        }
    }
}
