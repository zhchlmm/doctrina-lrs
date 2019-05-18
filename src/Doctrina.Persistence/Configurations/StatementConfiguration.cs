using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations
{
    public class StatementConfiguration : IEntityTypeConfiguration<StatementEntity>
    {
        public void Configure(EntityTypeBuilder<StatementEntity> builder)
        {
            builder.HasBaseType<StatementBaseEntity>();

            builder.Property(e => e.Stored)
               .IsRequired()
               .ValueGeneratedOnAdd();

            builder.Property(e => e.Version)
                .HasMaxLength(7);

            builder.HasOne(e => e.Authority)
                .WithMany()
                .HasPrincipalKey(e => e.AgentHash);

            builder.Property(e => e.Voided)
                .IsRequired()
                .HasDefaultValue(false);

            // Object SubStatement 
            builder.HasOne(r => r.ObjectSubStatement)
                .WithOne()
                .HasForeignKey<StatementEntity>(e => e.ObjectSubStatementId);
        }
    }
}
