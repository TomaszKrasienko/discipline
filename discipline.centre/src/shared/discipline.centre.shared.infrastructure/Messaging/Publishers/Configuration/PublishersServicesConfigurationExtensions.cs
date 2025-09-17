using discipline.centre.shared.infrastructure.Messaging.Abstractions;
using discipline.centre.shared.infrastructure.Messaging.Outbox.Configuration.Options;
using discipline.centre.shared.infrastructure.Messaging.Publishers.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace discipline.centre.shared.infrastructure.Messaging.Publishers.Configuration;

internal static class PublishersServicesConfigurationExtensions
{
    internal static IServiceCollection AddPublishers(this IServiceCollection services)
    {
        var outboxOptions = services.GetOptions<OutboxOptions>();

        if (outboxOptions.IsEnabled)
        {
            services.AddScoped<IMessagePublisher, OutboxMessagePublisher>();
        }
        
        services.AddScoped<IMessagePublisher, InternalAsyncMessagePublisher>();
        services.AddScoped<IMessageProcessor, MessageProcessor>();
        
        return services;
    }
}