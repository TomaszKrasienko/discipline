using discipline.activity_scheduler.domain.Accounts;
using discipline.activity_scheduler.domain.ActivityRules;
using discipline.activity_scheduler.domain.PlannedTasks;
using discipline.activity_scheduler.shared.abstractions.DAL;
using discipline.activity_scheduler.shared.abstractions.Identifiers;
using discipline.activity_scheduler.shared.abstractions.ViewModels;
using discipline.activity_scheduler.shared.abstractions.ViewModels.Abstractions;
using discipline.activity_scheduler.shared.infrastructure.DAL.Configuration.Options;
using discipline.activity_scheduler.shared.infrastructure.DAL.Repositories;
using discipline.libs.configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.activity_scheduler.shared.infrastructure.DAL.Configuration;

internal static class DalServicesConfigurationExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddOptions(configuration)
            .AddDbContext()
            .AddWriteRepository<Account, AccountId>()
            .AddWriteRepository<ActivityRule, ActivityRuleId>()
            .AddWriteRepository<PlannedTask, PlannedTaskId>()
            .AddReadRepository<AccountViewModel, AccountId>()
            .AddReadRepository<ActivityRuleViewModel, ActivityRuleId>()
            .AddReadRepository<PlannedTaskViewModel, PlannedTaskId>()
            .AddHostedService<PostgresDbMigrator>();

    private static IServiceCollection AddOptions(
        this IServiceCollection services,
        IConfiguration configuration)
        => services.ValidateAndBind<PostgresDbOptions, PostgresDbOptionsValidator>(configuration);

    private static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        var postgresOptions = services.GetOptions<PostgresDbOptions>();
        
        services.AddDbContext<Context>(options 
            => options.UseNpgsql(postgresOptions.ConnectionString));
        
        return services;
    }
    
    private static IServiceCollection AddWriteRepository<TEntity, TIdentifier>(
        this IServiceCollection services) where TEntity : class 
        => services.AddScoped<IWriteRepository<TEntity, TIdentifier>, WriteRepository<TEntity, TIdentifier>>();
    
    private static IServiceCollection AddReadRepository<TViewModel, TIdentifier>(
        this IServiceCollection services) where TViewModel : class, IViewModel
        => services.AddScoped<IReadRepository<TViewModel, TIdentifier>, ReadRepository<TViewModel, TIdentifier>>();
}