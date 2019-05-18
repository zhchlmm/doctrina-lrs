using Doctrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctrina.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(e => e.HomePage)
                .HasMaxLength(Constants.MAX_URL_LENGTH);

            builder.Property(e => e.Name)
               .HasMaxLength(40);

            builder
                .HasIndex(account => new { account.HomePage, account.Name })
                .HasFilter("[Account_HomePage] IS NOT NULL AND [Account_Name] IS NOT NULL")
                .IsUnique();
        }
    }
}
