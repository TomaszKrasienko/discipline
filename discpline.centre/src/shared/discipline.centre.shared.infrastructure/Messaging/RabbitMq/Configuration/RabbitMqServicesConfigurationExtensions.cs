using discipline.centre.shared.abstractions.Messaging;
using discipline.centre.shared.infrastructure.Configuration;
using discipline.centre.shared.infrastructure.Events.Brokers.RabbitMq;
using discipline.centre.shared.infrastructure.Messaging.Abstractions;
using discipline.centre.shared.infrastructure.Messaging.RabbitMq.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace discipline.centre.shared.infrastructure.Messaging.RabbitMq.Configuration;

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

        services.AddTransient<RabbitMqChannelFactory>();
        services.AddTransient<IMessagePublisher, RabbitMqMessagePublisher>();
        services.AddSingleton<IMessagesRouteRegistry, RabbitMqMessagesRouteRegistry>();

        return services;
    }
    
    public static IServiceCollection AddRabbitMqConsumer<TMessage>(this IServiceCollection services)
        where TMessage : class, IMessage
        => services.AddHostedService<RabbitMqConsumer<TMessage>>();
    
    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.ValidateAndBind<RabbitMqOptions, RabbitMqOptionsValidator>(configuration);
}