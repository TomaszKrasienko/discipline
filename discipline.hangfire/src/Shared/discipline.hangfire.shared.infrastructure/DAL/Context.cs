using discipline.hangfire.infrastructure.DAL.EntityTypesConfigurations;
using discipline.hangfire.infrastructure.DAL.EntityTypesConfigurations.Domain;
using discipline.hangfire.infrastructure.DAL.EntityTypesConfigurations.ViewModels;
using discipline.hangfire.infrastructure.DAL.ValueConverters;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace discipline.hangfire.infrastructure.DAL;

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