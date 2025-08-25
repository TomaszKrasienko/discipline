using discipline.centre.shared.infrastructure.Messaging.Outbox.Configuration.Options;
using discipline.centre.shared.infrastructure.Messaging.Outbox.DAL;
using discipline.centre.shared.infrastructure.Messaging.Publishers.Outbox.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class OutboxServicesConfigurationExtensions
{
    internal static IServiceCollection AddOutbox(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions(configuration);
        var outboxOptions = services.GetOptions<OutboxOptions>();

        if (outboxOptions.IsEnabled)
        {
            services.AddContext();
        }
        
        return services;
    }
    
    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<OutboxOptions>(configuration.GetSection(nameof(OutboxOptions)));

    private static IServiceCollection AddContext(this IServiceCollection services)
    {
        var outboxOptions = services.GetOptions<OutboxOptions>();

        if (string.IsNullOrWhiteSpace(outboxOptions.ConnectionString))
        {
            throw new InvalidOperationException("No connection string configured");
        }

        services.AddDbContext<OutboxDbContext>(x => x.UseNpgsql(
            outboxOptions.ConnectionString));

        services.AddHostedService<OutboxDatabaseMigrator>();
        
        return services;
    }
}