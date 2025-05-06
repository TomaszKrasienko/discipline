using discipline.hangfire.infrastructure.Identifiers;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace discipline.hangfire.add_activity_rules.DAL;

internal sealed class AddActivityRuleDbContext(DbContextOptions<AddActivityRuleDbContext> contextOptions)
    : DbContext(contextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("activity-rules");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<ActivityRuleId>()
            .HaveConversion<ActivityRuleIdValueConverter>();
        
        configurationBuilder
            .Properties<UserId>()
            .HaveConversion<UserIdValueConverter>();
    }
}