using discipline.centre.shared.infrastructure.Messaging.Abstractions;
using discipline.centre.shared.infrastructure.Messaging.Internal.Configuration;
using discipline.centre.shared.infrastructure.Messaging.Publishers.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.shared.infrastructure.Messaging.Configuration;

internal static class MessageServicesConfigurationExtensions
{
    internal static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration,
        string appName)
        => services
            .AddRabbitMq(configuration, appName)
            .AddInternalMessaging()
            .AddSingleton(typeof(IMessageHandler<>), typeof(MessageHandler<>))
            .AddOutbox(configuration)
            .AddPublishers();
}