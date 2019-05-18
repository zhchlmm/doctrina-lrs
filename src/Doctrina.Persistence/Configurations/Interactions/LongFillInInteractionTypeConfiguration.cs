using Doctrina.Domain.Entities.Interactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations.Interactions
{
    public class LongFillInInteractionTypeConfiguration : IEntityTypeConfiguration<LongFillInInteractionType>
    {
        public void Configure(EntityTypeBuilder<LongFillInInteractionType> builder)
        {
            builder.HasBaseType<AbstractInteractionType>();
        }
    }
}
