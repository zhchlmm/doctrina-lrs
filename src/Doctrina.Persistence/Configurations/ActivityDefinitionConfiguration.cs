using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations
{
    public class ActivityDefinitionConfiguration : IEntityTypeConfiguration<ActivityDefinitionEntity>
    {
        public void Configure(EntityTypeBuilder<ActivityDefinitionEntity> builder)
        {
            builder.Property(e => e.Name)
               .HasConversion<string>();

            builder.Property(e => e.Description)
                .HasConversion<string>();

            builder.Property(e => e.Type);

            builder.Property(e => e.MoreInfo);

            builder.Property(e => e.Extensions)
               .HasConversion<string>();

        }
    }
}
