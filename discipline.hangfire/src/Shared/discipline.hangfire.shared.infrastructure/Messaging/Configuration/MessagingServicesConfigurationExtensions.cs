using discipline.hangfire.infrastructure.Configuration;
using discipline.hangfire.infrastructure.Messaging.Abstractions;
using discipline.hangfire.infrastructure.Messaging.RabbitMq.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.hangfire.infrastructure.Messaging.Configuration;

internal static class MessagingServicesConfigurationExtensions
{
    internal static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
        => services
            .ValidateAndBind<MessagingOptions, MessagingOptionsValidator>(configuration)
            .AddSingleton<IConventionProvider, RabbitMqConventionProvider>()
            .AddSingleton<IMessagesRouteRegistry, RabbitMqMessagesRouteRegistry>()
            .AddRabbitMq(configuration);
}