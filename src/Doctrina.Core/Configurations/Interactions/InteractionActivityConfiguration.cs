using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Persistence.Configurations
{
    public class InteractionActivityConfiguration : IEntityTypeConfiguration<InteractionActivity>
    {
        public void Configure(EntityTypeBuilder<InteractionActivity> builder)
        {
            builder.HasBaseType<ActivityDefinitionEntity>();

            builder.Property(e => e.CorrectResponsesPattern)
                .HasConversion<string>();

            builder.Property(e => e.InteractionType)
                .HasMaxLength(12)
                .IsRequired();
        }
    }
}
