using discipline.libs.configuration;
using discipline.libs.messaging.Abstractions;
using discipline.libs.rabbit_mq;
using discipline.libs.rabbit_mq.Configurations.Options;
using discipline.libs.rabbit_mq.Configurations.Options.Validators;
using discipline.libs.rabbit_mq.Connections;
using discipline.libs.rabbit_mq.Conventions;
using discipline.libs.rabbit_mq.Factories;
using discipline.libs.rabbit_mq.Registries;
using discipline.libs.rabbit_mq.Registries.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class RabbitMqServicesConfigurationExtensions
{
    public static IServiceCollection AddRabbitMq(
        this IServiceCollection services,
        IConfiguration configuration,
        string appName)
        => services
            .AddOptions(configuration)
            .AddSerialization()
            .AddConnections(appName)
            .AddTransient<RabbitMqChannelFactory>()
            .AddSingleton<IConventionProvider, RabbitMqConventionProvider>()
            .AddSingleton<IMessagesRouteRegistry, RabbitMqMessagesRouteRegistry>()
            .AddTransient<IMessagePublisher, RabbitMqMessagePublisher>();
        
    private static IServiceCollection AddConnections(
        this IServiceCollection services,
        string appName)
    {
        services.AddSingleton(sp =>
        {
            var rabbitMqOptions = sp.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
            
            var factory = new ConnectionFactory
            {
                HostName = rabbitMqOptions.HostName,
                UserName = rabbitMqOptions.Username,
                Password = rabbitMqOptions.Password,
                VirtualHost = rabbitMqOptions.VirtualHost,
                Port = rabbitMqOptions.Port
            };

            var consumerConnection = factory.CreateConnectionAsync($"{appName}.Consumer").GetAwaiter().GetResult();
            var producerConnection = factory.CreateConnectionAsync($"{appName}.Producer").GetAwaiter().GetResult();
            
            return new RabbitMqConnectionProvider(consumerConnection, producerConnection);
        });
        
        return services;
    }
    
    private static IServiceCollection AddOptions(
        this IServiceCollection services,
        IConfiguration configuration)
        => services.ValidateAndBind<RabbitMqOptions, RabbitMqOptionsValidator>(configuration);

    public static IServiceCollection AddConsumer<TMessage>(
        this IServiceCollection services,
        Func<IServiceProvider, Func<TMessage, CancellationToken, string?, Task>> handle) where TMessage : class, IMessage
    {
        services.AddHostedService(sp =>
        {
            var handler = handle(sp);
            return new RabbitMqConsumer<TMessage>(sp, handler);
        });
        return services;
    }
}