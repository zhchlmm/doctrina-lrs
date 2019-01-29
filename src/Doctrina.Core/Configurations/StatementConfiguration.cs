using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Persistence.Configurations
{
    public class StatementConfiguration : IEntityTypeConfiguration<StatementEntity>
    {
        public void Configure(EntityTypeBuilder<StatementEntity> builder)
        {
            builder.HasBaseType(typeof(StatementBaseEntity));

            builder.HasKey(e => e.StatementId);
            builder.Property(e => e.StatementId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Stored)
               .IsRequired();

            builder.Property(e => e.Version)
                .HasMaxLength(7);

            builder.HasOne(e => e.Authority)
                .WithMany()
                .HasForeignKey(e => e.AuthorityId);

            builder.HasMany(e => e.Attachments)
                .WithOne()
                .HasForeignKey(e => e.StatementId);

            builder.Property(e => e.Voided)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
