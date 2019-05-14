using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations
{
    public class SubStatementConfiguration : IEntityTypeConfiguration<SubStatementEntity>
    {
        public void Configure(EntityTypeBuilder<SubStatementEntity> builder)
        {
            builder.HasBaseType(typeof(StatementBaseEntity));

            builder.HasKey(e => e.SubStatementId);
            builder.Property(e => e.SubStatementId)
                .ValueGeneratedOnAdd();

            //builder.HasOne(e => e.ObjectStatementRef)
            //    .WithMany()
            //    .HasForeignKey(e => e.ObjectStatementRefId);
        }
    }
}
