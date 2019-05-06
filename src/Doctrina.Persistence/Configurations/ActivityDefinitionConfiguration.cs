using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations
{
    public class ActivityDefinitionConfiguration : IEntityTypeConfiguration<ActivityDefinitionEntity>
    {
        public void Configure(EntityTypeBuilder<ActivityDefinitionEntity> builder)
        {
            builder.OwnsMany(p => p.Name, a => {
                a.HasForeignKey("ActivityDefinitionId");
                a.Property<int>("Id");
                a.HasKey("ActivityDefinitionId", "Id");
            });

            builder.OwnsMany(p => p.Description, a => {
                a.HasForeignKey("ActivityDefinitionId");
                a.Property<int>("Id");
                a.HasKey("ActivityDefinitionId", "Id");
            });

            builder.Property(e => e.Type);

            builder.Property(e => e.MoreInfo);

            builder.OwnsMany(p => p.Extensions, a => {
                a.HasForeignKey("ActivityDefinitionId");
                a.Property<int>("Id");
                a.HasKey("ActivityDefinitionId", "Id");
            });
        }
    }
}
