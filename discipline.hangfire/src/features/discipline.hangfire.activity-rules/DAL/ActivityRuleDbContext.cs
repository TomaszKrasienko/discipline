using discipline.hangfire.activity_rules.DAL.EntityTypeConfiguration;
using discipline.hangfire.infrastructure.Identifiers;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace discipline.hangfire.activity_rules.DAL;

internal sealed class ActivityRuleDbContext(DbContextOptions<ActivityRuleDbContext> contextOptions)
    : DbContext(contextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("activity-rules");
        modelBuilder.ApplyConfiguration(new ActivityRuleTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ActivityRuleViewModelConfiguration());
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