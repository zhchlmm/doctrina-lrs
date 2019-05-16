using Doctrina.Domain.Entities.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Doctrina.Persistence.Configurations.Documents
{
    public class DocumentConfiguration : IEntityTypeConfiguration<DocumentEntity>
    {
        public void Configure(EntityTypeBuilder<DocumentEntity> builder)
        {
            builder.Property(e => e.ContentType)
                .HasMaxLength(255);

            builder.Property(e => e.Content);

            builder.Property(e => e.Checksum)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.LastModified)
                .HasDefaultValue(DateTime.UtcNow)
                .IsRequired();
        }
    }
}
