using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations
{
    public class StatementBaseConfiguration : IEntityTypeConfiguration<StatementBaseEntity>
    {
        public void Configure(EntityTypeBuilder<StatementBaseEntity> builder)
        {
            builder.ToTable("StatementBase");

            builder.HasKey(e => e.StatementId);
            builder.Property(e => e.StatementId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.ObjectObjectType)
                .HasColumnType("nvarchar(12)")
                .IsRequired()
                .HasConversion<string>();

            // Actor
            builder.HasOne(e => e.Actor)
                .WithMany()
                .HasPrincipalKey(e=> e.AgentHash)
                .IsRequired();

            // Verb
            builder.HasOne(e => e.Verb)
                .WithMany()
                .IsRequired();

            // Object Agent 
            builder.HasOne(r => r.ObjectAgent)
                .WithMany()
                .HasPrincipalKey(x=> x.AgentHash);

            // Object Activity 
            builder.HasOne(r => r.ObjectActivity)
                .WithMany()
                .HasPrincipalKey(x=> x.ActivityHash);

            // Object SubStatement 
            //builder.HasOne(r => r.ObjectSubStatement)
            //    .WithOne()
            //    .HasForeignKey<StatementEntity>(e => e.ObjectSubStatementId);

            builder.HasOne(e => e.Result)
                .WithOne(x=> x.Statement)
                .HasForeignKey<ResultEntity>(x=> x.StatementId);

            builder.Property(e => e.Timestamp)
                .IsRequired();

            builder.HasMany(e => e.Attachments)
                .WithOne(x=> x.Statement)
                .HasForeignKey(e => e.StatementId);
        }
    }
}
