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
                .WithOne();

            builder.HasMany(e => e.Grouping)
                .WithOne();

            builder.HasMany(e => e.Category)
                .WithOne();

            builder.HasMany(e => e.Other)
                .WithOne();
        }
    }
}
