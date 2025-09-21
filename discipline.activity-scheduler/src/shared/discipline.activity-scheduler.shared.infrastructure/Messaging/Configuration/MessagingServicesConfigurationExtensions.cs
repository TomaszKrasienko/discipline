using discipline.activity_scheduler.shared.abstractions.Messaging;
using discipline.activity_scheduler.shared.infrastructure.Configuration.Options;
using discipline.libs.configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.activity_scheduler.shared.infrastructure.Messaging.Configuration;

internal static class MessagingServicesConfigurationExtensions
{
    internal static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var appOptions = services.GetOptions<AppOptions>();
        
        return services
            .AddRabbitMq(configuration, appOptions.Name)
            .AddTransient<IMessageProcessor, MessageProcessor>();
    }
}