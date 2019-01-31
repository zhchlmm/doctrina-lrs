using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations
{
    public class ContextActivitiesConfiguration : IEntityTypeConfiguration<ContextActivitiesEntity>
    {
        public void Configure(EntityTypeBuilder<ContextActivitiesEntity> builder)
        {
            builder.ToTable("ContextActivities");

            builder.HasKey(e => e.ContextActivitiesId);
            builder.Property(e => e.ContextActivitiesId)
                .ValueGeneratedOnAdd();

            builder.HasMany(e => e.Parent)
                .WithOne()
                .HasForeignKey(e=> e.ContextId);

            builder.HasMany(e => e.Grouping)
                .WithOne()
                .HasForeignKey(e => e.ContextId);

            builder.HasMany(e => e.Category)
                .WithOne()
                .HasForeignKey(e => e.ContextId);

            builder.HasMany(e => e.Other)
                .WithOne()
                .HasForeignKey(e => e.ContextId);
        }
    }
}
