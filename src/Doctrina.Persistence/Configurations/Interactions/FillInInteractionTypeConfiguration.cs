using Doctrina.Domain.Entities.Interactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations.Interactions
{
    public class FillInInteractionTypeConfiguration : IEntityTypeConfiguration<FillInInteractionType>
    {
        public void Configure(EntityTypeBuilder<FillInInteractionType> builder)
        {
            builder.HasBaseType<AbstractInteractionType>();
        }
    }
}
