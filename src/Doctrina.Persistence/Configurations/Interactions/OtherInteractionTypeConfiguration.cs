using Doctrina.Domain.Entities;
using Doctrina.Domain.Entities.Interactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations.Interactions
{
    public class OtherInteractionTypeConfiguration : IEntityTypeConfiguration<OtherInteractionType>
    {
        public void Configure(EntityTypeBuilder<OtherInteractionType> builder)
        {
            builder.HasBaseType<AbstractInteractionType>();
        }
    }
}
