using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations
{
    public class ScoreConfiguration : IEntityTypeConfiguration<ScoreEntity>
    {
        public void Configure(EntityTypeBuilder<ScoreEntity> builder)
        {
            builder.Property(e => e.Scaled);
            builder.Property(e => e.Raw);
            builder.Property(e => e.Min);
            builder.Property(e => e.Max);
        }
    }
}
