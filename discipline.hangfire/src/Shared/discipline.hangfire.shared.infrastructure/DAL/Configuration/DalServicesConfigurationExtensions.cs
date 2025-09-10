using discipline.hangfire.domain.Accounts;
using discipline.hangfire.domain.ActivityRules;
using discipline.hangfire.domain.PlannedTasks;
using discipline.hangfire.infrastructure.DAL.Configuration.Options;
using discipline.hangfire.infrastructure.DAL.Repositories;
using discipline.hangfire.shared.abstractions.DAL;
using discipline.hangfire.shared.abstractions.Identifiers;
using discipline.hangfire.shared.abstractions.ViewModels;
using discipline.hangfire.shared.abstractions.ViewModels.Abstractions;
using discipline.libs.configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.hangfire.infrastructure.DAL.Configuration;

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