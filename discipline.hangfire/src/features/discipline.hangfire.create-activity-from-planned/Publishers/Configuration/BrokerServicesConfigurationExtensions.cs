using discipline.hangfire.create_activity_from_planned.Publishers.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.hangfire.create_activity_from_planned.Publishers.Configuration;

internal static class BrokerServicesConfigurationExtensions
{
    internal static IServiceCollection AddBroker(this IServiceCollection services)
        => services
            .AddScoped<IBrokerPublisher, RedisBrokerPublisher>();
}