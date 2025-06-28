using discipline.hangfire.infrastructure.Configuration;
using discipline.hangfire.infrastructure.Configuration.Options;
using discipline.hangfire.infrastructure.Messaging.RabbitMq.Abstractions;
using discipline.hangfire.shared.abstractions.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace discipline.hangfire.infrastructure.Messaging.RabbitMq.Configuration;

public static class RabbitMqServicesConfigurationExtensions
{
    internal static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions(configuration);

        services.AddSingleton(sp =>
        {
            var rabbitMqOptions = sp.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
            var appOptions = sp.GetRequiredService<IOptions<AppOptions>>().Value;
            
            var factory = new ConnectionFactory
            {
                HostName = rabbitMqOptions.HostName,
                UserName = rabbitMqOptions.Username,
                Password = rabbitMqOptions.Password,
                VirtualHost = rabbitMqOptions.VirtualHost,
                Port = rabbitMqOptions.Port
            };

            var consumerConnection = factory.CreateConnectionAsync($"{appOptions.Name}.Consumer").GetAwaiter().GetResult();
            var producerConnection = factory.CreateConnectionAsync($"{appOptions.Name}.Producer").GetAwaiter().GetResult();
            
            return new RabbitMqConnectionProvider(consumerConnection, producerConnection);
        });

        services
            .AddTransient<RabbitMqChannelFactory>()
            .AddSingleton<IConventionProvider, RabbitMqConventionProvider>()
            .AddSingleton<IMessagesRouteRegistry, RabbitMqMessagesRouteRegistry>();

        return services;
    }

    public static IServiceCollection AddRabbitMqConsumer<TEvent>(this IServiceCollection services)
        where TEvent : class, IEvent
        => services.AddHostedService<RabbitMqConsumer<TEvent>>();
    
    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.ValidateAndBind<RabbitMqOptions, RabbitMqOptionsValidator>(configuration);
}