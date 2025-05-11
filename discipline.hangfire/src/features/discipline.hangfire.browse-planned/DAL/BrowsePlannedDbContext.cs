

using discipline.hangfire.infrastructure.Identifiers;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace discipline.hangfire.browse_planned.DAL;

internal sealed class BrowsePlannedDbContext(DbContextOptions<BrowsePlannedDbContext> contextOptions)
    : DbContext(contextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("browse-planned");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<UserId>()
            .HaveConversion<UserIdValueConverter>();
        
        configurationBuilder
            .Properties<ActivityRuleId>()
            .HaveConversion<ActivityRuleIdValueConverter>();
    }
}