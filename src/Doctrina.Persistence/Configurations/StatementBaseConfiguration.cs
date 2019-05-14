using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations
{
    public class StatementBaseConfiguration : IEntityTypeConfiguration<StatementBaseEntity>
    {
        public void Configure(EntityTypeBuilder<StatementBaseEntity> builder)
        {
            //builder.HasKey(e => e.StatementBaseId);
            //builder.Property(e => e.StatementBaseId)
            //    .ValueGeneratedOnAdd();

            builder.Property(e => e.ObjectObjectType)
                .HasColumnType("nvarchar(12)")
                .IsRequired()
                .HasConversion<string>();

            // Actor
            builder.Property(e => e.Actor)
                .IsRequired();
            builder.HasOne(e => e.Actor)
                .WithMany();

            // Verb
            builder.Property(e => e.Verb)
                .IsRequired();
            builder.HasOne(e => e.Verb)
                .WithMany();

            // Object Agent 
            builder.HasOne(r => r.ObjectAgent)
                .WithMany();

            // Object Activity 
            builder.HasOne(r => r.ObjectActivity)
                .WithMany();

            // Object SubStatement 
            //builder.HasOne(r => r.ObjectSubStatement)
            //    .WithOne()
            //    .HasForeignKey<StatementEntity>(e => e.ObjectSubStatementId);

            builder.HasOne(e => e.Result)
                .WithOne();

            builder.Property(e => e.Timestamp)
                .IsRequired();

            builder.HasMany(e => e.Attachments)
                .WithOne()
                .HasForeignKey(e => e.StatementId);
        }
    }
}
