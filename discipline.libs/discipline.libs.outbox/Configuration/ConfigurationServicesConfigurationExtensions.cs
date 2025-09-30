using discipline.libs.outbox.Configuration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.libs.outbox.Configuration;

public static class ConfigurationServicesConfigurationExtensions
{
    public static IServiceCollection AddOutbox(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services;
    }
}