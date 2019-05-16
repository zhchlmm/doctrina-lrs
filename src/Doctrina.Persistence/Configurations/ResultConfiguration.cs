using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations
{
    public class ResultConfiguration : IEntityTypeConfiguration<ResultEntity>
    {
        public void Configure(EntityTypeBuilder<ResultEntity> builder)
        {
            builder.ToTable("Result");

            builder.HasKey(e => e.ResultId);
            builder.Property(e => e.ResultId)
                .ValueGeneratedOnAdd();

            builder.HasOne(e => e.Score)
                .WithOne()
                .HasForeignKey<ResultEntity>(e => e.ScoreId);

            builder.Property(e => e.Extensions)
                .HasColumnType("nvarchar(max)");
        }
    }
}
