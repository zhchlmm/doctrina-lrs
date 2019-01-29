using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations
{
    public class ActivityConfiguration : IEntityTypeConfiguration<ActivityEntity>
    {
        public void Configure(EntityTypeBuilder<ActivityEntity> builder)
        {
            builder.HasKey(x => x.Key);
            builder.Property(e => e.Key)
                .ValueGeneratedOnAdd();

            builder.HasIndex(ac => ac.Id);
            builder.Property(e => e.Id)
                .HasMaxLength(Constants.MAX_URL_LENGTH)
                .IsRequired();

            builder.HasOne(x => x.Definition)
                .WithOne()
                .HasForeignKey<ActivityEntity>(e=> e.DefinitionId);
        }
    }
}
