using discipline.activity_scheduler.shared.abstractions.Identifiers;
using discipline.activity_scheduler.shared.infrastructure.DAL.EntityTypesConfigurations.Domain;
using discipline.activity_scheduler.shared.infrastructure.DAL.EntityTypesConfigurations.ViewModels;
using discipline.activity_scheduler.shared.infrastructure.DAL.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace discipline.activity_scheduler.shared.infrastructure.DAL;

public sealed class Context(DbContextOptions<Context> options) : DbContext(options) 
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AccountTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ActivityRuleTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PlannedTaskTypeConfiguration());

        modelBuilder.ApplyConfiguration(new AccountViewModelTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ActivityRuleViewModelTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PlannedTaskViewModelTypeConfiguration());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<AccountId>()
            .HaveConversion<AccountIdValueConverter>();
        
        configurationBuilder
            .Properties<ActivityRuleId>()
            .HaveConversion<ActivityRuleIdValueConverter>();
        
        configurationBuilder
            .Properties<PlannedTaskId>()
            .HaveConversion<PlannedTaskIdValueConverter>();
    }
}