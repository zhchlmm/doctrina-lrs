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

            builder.Property(e => e.ObjectType)
                .HasColumnType("nvarchar(12)")
                .IsRequired()
                .HasConversion<string>();

            // Actor
            builder.Property(e => e.ActorKey)
                .IsRequired();
            builder.HasOne(e => e.Actor)
                .WithMany()
                .HasForeignKey(e => e.ActorKey);

            // Verb
            builder.Property(e => e.VerbKey)
                .IsRequired();
            builder.HasOne(e => e.Verb)
                .WithMany()
                .HasForeignKey(e => e.VerbKey);

            // Object Agent 
            builder.HasOne(r => r.ObjectAgent)
                .WithMany()
                .HasForeignKey(e=> e.ObjectAgentKey);

            // Object Activity 
            builder.HasOne(r => r.ObjectActivity)
                .WithMany()
                .HasForeignKey(e => e.ObjectActivityKey);

            // Object SubStatement 
            builder.HasOne(r => r.ObjectSubStatement)
                .WithOne()
                .HasForeignKey<StatementEntity>(e => e.ObjectSubStatementId);

            builder.HasOne(e => e.Result)
                .WithOne()
                .HasForeignKey<StatementEntity>(e=> e.ResultId);

            builder.Property(e => e.Timestamp)
                .IsRequired();

            builder.HasMany(e => e.Attachments)
                .WithOne()
                .HasForeignKey(e => e.StatementId);
        }
    }
}
