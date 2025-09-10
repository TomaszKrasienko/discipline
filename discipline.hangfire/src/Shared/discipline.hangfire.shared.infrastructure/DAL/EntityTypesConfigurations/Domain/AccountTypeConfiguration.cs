using discipline.hangfire.domain.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace discipline.hangfire.infrastructure.DAL.EntityTypesConfigurations.Domain;

internal sealed class AccountTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder
            .HasKey(x => x.AccountId);
        
        builder
            .Property(x => x.AccountId)
            .HasColumnName("AccountId")
            .IsRequired()
            .HasMaxLength(26);
    }
}