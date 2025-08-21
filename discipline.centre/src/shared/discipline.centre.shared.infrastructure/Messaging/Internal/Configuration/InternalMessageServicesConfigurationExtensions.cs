using discipline.centre.shared.infrastructure.Messaging.Abstractions;
using discipline.centre.shared.infrastructure.Messaging.Internal.Channels;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.shared.infrastructure.Messaging.Internal.Configuration;

internal static class InternalMessageServicesConfigurationExtensions
{
    internal static IServiceCollection AddInternalMessaging(this IServiceCollection services)
        => services
            .AddSingleton<IMessageChannel, MessageChannel>()
            .AddSingleton<IMessagePublisher, InternalAsyncMessageDispatcher>()
            .AddHostedService<BackgroundMessageDispatcher>();
}