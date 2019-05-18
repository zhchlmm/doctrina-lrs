using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Doctrina.Persistence.Configurations
{
    public class ResultConfiguration : IEntityTypeConfiguration<ResultEntity>
    {
        public void Configure(EntityTypeBuilder<ResultEntity> builder)
        {
            builder.Property(e => e.ResultId)
                .ValueGeneratedOnAdd();
            builder.HasKey(e => e.ResultId);

            builder.OwnsOne(e => e.Score, a =>
            {
                a.HasForeignKey("ResultId");
                a.Property<Guid>("ScoreId");
                a.HasKey("ScoreId");
                a.ToTable("Result_Scores");
            });

            builder.OwnsMany(e => e.Extensions, a => {
                a.HasForeignKey("ResultId");
                a.Property<Guid>("ExtensionId");
                a.HasKey("ExtensionId");
            });
        }
    }
}
