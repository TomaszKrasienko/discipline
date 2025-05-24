using discipline.centre.shared.infrastructure.Messaging.Internal.Configuration;
using discipline.centre.shared.infrastructure.Messaging.RabbitMq;
using discipline.centre.shared.infrastructure.Messaging.RabbitMq.Abstractions;
using discipline.centre.shared.infrastructure.Messaging.RabbitMq.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.shared.infrastructure.Messaging.Configuration;

internal static class MessageServicesConfigurationExtensions
{
    internal static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddRabbitMq(configuration)
            .AddInternalMessaging()
            .AddSingleton<IMessageConventionProvider, MessageConventionProvider>();
}