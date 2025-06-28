using discipline.hangfire.server.Hangfire;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class HangfireServicesConfigurationExtensions
{
    internal static IServiceCollection AddDisciplineHangfire(this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddPostgresOptions(configuration)
            .AddServices()
            .AddHangfireServer();
    
    private static IServiceCollection AddPostgresOptions(this IServiceCollection services, IConfiguration configuration)
        => services
            .Configure<PostgresHangfireOptions>(configuration.GetSection(nameof(PostgresHangfireOptions)));

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();
        using var scope = sp.CreateScope();
        var postgresOptions = scope.ServiceProvider.GetRequiredService<IOptions<PostgresHangfireOptions>>();
        services
            .AddHangfire(configuration =>
                configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UsePostgreSqlStorage(connection =>
                    {
                        connection.UseNpgsqlConnection(postgresOptions.Value.ConnectionString);
                    }));
        
        return services;
    }
}