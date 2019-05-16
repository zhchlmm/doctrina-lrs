using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations
{
    public class AttachmentConfiguration : IEntityTypeConfiguration<AttachmentEntity>
    {
        public void Configure(EntityTypeBuilder<AttachmentEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.UsageType)
                .IsRequired()
                .HasMaxLength(Constants.MAX_URL_LENGTH);

            builder.Property(e => e.ContentType)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.SHA2)
                .IsRequired();

            builder.Property(e => e.CanonicalData)
                .HasColumnType("ntext");

            builder.Property(e => e.Length)
                .IsRequired();

            builder.HasOne(e => e.Statement)
                .WithOne()
                .HasForeignKey<AttachmentEntity>(e => e.StatementId);
        }
    }
}
