using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations
{
    public class AgentConfiguration : IEntityTypeConfiguration<AgentEntity>
    {
        public void Configure(EntityTypeBuilder<AgentEntity> builder)
        {
            builder.Property(x=> x.AgentHash)
                .IsRequired();
            builder.HasKey(x => x.AgentHash);

            //builder.Property(e => e.ObjectType)
            //    .IsRequired()
            //    .HasMaxLength(Constants.OBJECT_TYPE_LENGTH);// Or just 5 = Agent or Group

            builder.HasDiscriminator(x => x.ObjectType)
                .HasValue<AgentEntity>(EntityObjectType.Agent)
                .HasValue<GroupEntity>(EntityObjectType.Group);

            builder.Property(e => e.Name)
                .HasMaxLength(100);

            builder.Property(e => e.Mbox)
                .HasMaxLength(128);

            builder.Property(e => e.Mbox_SHA1SUM)
                .HasMaxLength(40);

            builder.OwnsOne(e => e.Account).Property(x => x.HomePage).HasColumnName("Account_HomePage");
            builder.OwnsOne(e => e.Account).Property(x => x.Name).HasColumnName("Account_Name");

            //builder
            //    .HasIndex(x => new { x.ObjectType, x.Mbox })
            //    .HasFilter("[Mbox] IS NOT NULL")
            //    .IsUnique();

            //builder
            //    .HasIndex(x => new { x.ObjectType, x.Mbox_SHA1SUM })
            //    .HasFilter("[Mbox_SHA1SUM] IS NOT NULL")
            //    .IsUnique();

            //builder
            //    .HasIndex(x => new { x.ObjectType, x.OpenId })
            //    .HasFilter("[OpenId] IS NOT NULL")
            //    .IsUnique();
        }
    }
}
