using Doctrina.Domain.Entities.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Persistence.Configurations.Documents
{
    public class ActivityProfileConfiguration : IEntityTypeConfiguration<ActivityProfileEntity>
    {
        public void Configure(EntityTypeBuilder<ActivityProfileEntity> builder)
        {
            builder.HasKey(e => e.Key);
            builder.Property(e => e.Key)
                .ValueGeneratedOnAdd();

        }
    }
}
