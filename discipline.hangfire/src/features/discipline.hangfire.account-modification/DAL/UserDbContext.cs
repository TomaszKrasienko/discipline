using discipline.hangfire.account_modification.DAL.EntityTypeConfiguration;
using discipline.hangfire.infrastructure.Identifiers;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace discipline.hangfire.account_modification.DAL;

internal sealed class AccountDbContext(DbContextOptions<AccountDbContext> contextOptions)
    : DbContext(contextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("accounts");
        modelBuilder.ApplyConfiguration(new AccountTypeConfiguration());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<AccountId>()
            .HaveConversion<AccountIdValueConverter>();
    }
}