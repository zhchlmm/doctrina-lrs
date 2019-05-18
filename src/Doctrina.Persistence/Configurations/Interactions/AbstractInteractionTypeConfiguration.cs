using Doctrina.Domain.Entities;
using Doctrina.Domain.Entities.Interactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Doctrina.Persistence.Configurations
{
    public class AbstractInteractionTypeConfiguration : IEntityTypeConfiguration<AbstractInteractionType>
    {
        public void Configure(EntityTypeBuilder<AbstractInteractionType> builder)
        {
            builder.ToTable("InteractionActivities");

            builder.HasBaseType<ActivityDefinitionEntity>();

            builder.Property<int>("InteractionId")
                .ValueGeneratedOnAdd();

            var StringArrConverter = new ValueConverter<string[], string>(
                v=> string.Join('|', v),
                v=> v.Split(new char[] { '|' })
                );

            builder.Property(e => e.CorrectResponsesPattern)
                .HasConversion(StringArrConverter);

            builder.Property(e => e.InteractionType)
                .HasMaxLength(12)
                .IsRequired();
        }
    }
}
