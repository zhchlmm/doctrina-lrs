using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations
{
    public class ContextConfiguration : IEntityTypeConfiguration<ContextEntity>
    {
        public void Configure(EntityTypeBuilder<ContextEntity> builder)
        {
            builder.ToTable("Context");

            builder.HasKey(e => e.ContextId);
            builder.Property(e => e.ContextId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Extensions)
                .HasColumnType("ntext");

            builder.HasOne(e => e.Instructor)
                .WithMany()
                .HasForeignKey(e=> e.InstructorId);

            builder.HasOne(e => e.Instructor)
                .WithMany()
                .HasForeignKey(e => e.InstructorId);

            builder.HasOne(e => e.ContextActivities)
                .WithMany()
                .HasForeignKey(e => e.ContextActivitiesId);
        }
    }
}
