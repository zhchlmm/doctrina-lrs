using Doctrina.Domain.Entities.Interactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations.Interactions
{
    public class NumericInteractionTypeConfiguration : IEntityTypeConfiguration<NumericInteractionType>
    {
        public void Configure(EntityTypeBuilder<NumericInteractionType> builder)
        {
            builder.HasBaseType<AbstractInteractionType>();
        }
    }
}
