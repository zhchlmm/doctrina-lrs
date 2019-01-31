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
    public class ScoreConfiguration : IEntityTypeConfiguration<ScoreEntity>
    {
        public void Configure(EntityTypeBuilder<ScoreEntity> builder)
        {
            builder.HasKey(e => e.ScoreId);
            builder.Property(e => e.ScoreId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Scaled);

            builder.HasOne(e => e.Result)
                .WithOne()
                .HasForeignKey<ResultEntity>(e => e.ResultId);
        }
    }
}
