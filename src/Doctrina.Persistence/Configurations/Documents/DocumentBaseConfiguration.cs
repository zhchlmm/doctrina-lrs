using Doctrina.Domain.Entities.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Doctrina.Persistence.Configurations.Documents
{
    public class DocumentConfiguration : IEntityTypeConfiguration<DocumentBaseEntity>
    {
        public void Configure(EntityTypeBuilder<DocumentBaseEntity> builder)
        {
            builder.ToTable("Documents");

            builder.HasKey(x => x.Key);
            builder.Property(x => x.Key)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.ContentType)
                .HasMaxLength(255);

            builder.Property(e => e.Content);

            builder.Property(e => e.Checksum)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.LastModified)
                .HasDefaultValue(DateTimeOffset.UtcNow)
                .IsRequired()
                .ValueGeneratedOnAddOrUpdate();

            builder.Property(e => e.CreateDate)
                .ValueGeneratedOnAdd();
        }
    }
}
