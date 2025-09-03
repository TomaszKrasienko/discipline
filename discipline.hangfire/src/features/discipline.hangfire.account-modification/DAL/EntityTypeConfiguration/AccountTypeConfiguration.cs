using discipline.hangfire.account_modification.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace discipline.hangfire.account_modification.DAL.EntityTypeConfiguration;

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