using Doctrina.Domain.Entities;
using Doctrina.Persistence.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Doctrina.Persistence.Configurations
{
    public class VerbConfiguration : IEntityTypeConfiguration<VerbEntity>
    {
        public void Configure(EntityTypeBuilder<VerbEntity> builder)
        {
            builder.ToTable("Verbs");

            builder.Property(e => e.VerbHash)
                .IsRequired()
                .HasMaxLength(32);
            builder.HasKey(e => e.VerbHash);

            builder.Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(Constants.MAX_URL_LENGTH);

            builder.Property(p => p.Display)
                .HasConversion(new LanguageMapCollectionValueConverter())
                .HasColumnType("ntext");
        }
    }
}
