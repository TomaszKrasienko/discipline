using discipline.hangfire.infrastructure.Configuration;
using discipline.hangfire.infrastructure.Messaging.RabbitMq;
using discipline.hangfire.infrastructure.Messaging.RabbitMq.Abstractions;
using discipline.hangfire.infrastructure.Messaging.RabbitMq.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.hangfire.infrastructure.Messaging.Configuration;

internal static class MessagingServicesConfigurationExtensions
{
    internal static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddRabbitMq(configuration);
}