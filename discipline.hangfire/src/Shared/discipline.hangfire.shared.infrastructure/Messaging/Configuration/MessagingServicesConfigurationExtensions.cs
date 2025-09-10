using discipline.hangfire.infrastructure.Configuration.Options;
using discipline.hangfire.shared.abstractions.Messaging;
using discipline.libs.configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.hangfire.infrastructure.Messaging.Configuration;

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