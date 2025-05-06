using discipline.hangfire.infrastructure.Configuration;
using discipline.hangfire.infrastructure.Postgres;
using discipline.hangfire.infrastructure.Postgres.Configuration;
using discipline.hangfire.shared.abstractions.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class PostgreServicesConfigurationExtensions
{
    internal static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddOptions(configuration)
            .AddServices();
    
    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services
            .Configure<PostgresBusinessOptions>(configuration.GetSection(nameof(PostgresBusinessOptions)));

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddTransient<IDbContext, PostgreSqlDbContext>()
            .AddTransient<IDbConnectionFactory, PostgresDbConnectionFactory>()
            .AddTransient<IDbTransactionManager, DbTransactionManager>();
    
    public static IServiceCollection AddContext<TContext>(this IServiceCollection services) where TContext : DbContext
    {
        services.AddHostedService<DatabaseMigrator<TContext>>();
        var options = services.GetOptions<PostgresBusinessOptions>();
        services.AddDbContext<TContext>(x => x.UseNpgsql(options.ConnectionString));
        return services;
    }
}