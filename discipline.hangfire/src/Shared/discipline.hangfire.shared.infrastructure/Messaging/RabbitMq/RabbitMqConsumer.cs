using discipline.hangfire.infrastructure.Messaging.RabbitMq.Abstractions;
using discipline.hangfire.shared.abstractions.Events;
using discipline.hangfire.shared.abstractions.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace discipline.hangfire.infrastructure.Messaging.RabbitMq;

internal sealed class RabbitMqConsumer<TEvent>(
    RabbitMqChannelFactory rabbitMqChannelFactory,
    ISerializer serializer,
    IServiceProvider serviceProvider,
    IMessagesRouteRegistry routeRegistry,
    IConventionProvider conventionProvider,
    ILogger<RabbitMqConsumer<TEvent>> logger) : IHostedService where TEvent : class, IEvent
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var channel = rabbitMqChannelFactory.ConsumerChannel;
        var consumer = new AsyncEventingBasicConsumer(channel);

        var (exchange, routingKeys) = routeRegistry.GetRoute<TEvent>();
        var queue = conventionProvider.GetQueue<TEvent>();

        await channel.ExchangeDeclareAsync(
            exchange: exchange,
            type: ExchangeType.Direct,
            durable: false,
            autoDelete: false,
            cancellationToken: cancellationToken);
        
        await channel.QueueDeclareAsync(
            queue: queue,
            durable: false,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        foreach (var routingKey in routingKeys)
        {
            await channel.QueueBindAsync(
                queue: queue,
                exchange: exchange,
                routingKey: routingKey,
                cancellationToken: cancellationToken);
        }

        consumer.ReceivedAsync += async (sender, ea) =>
        {
            try
            {
                var scope = serviceProvider.CreateAsyncScope();
                var eventDispatcher = scope.ServiceProvider.GetRequiredService<IEventDispatcher>();
                
                var message = serializer.ToObject<TEvent>(ea.Body.ToArray());

                if (message is null)
                {
                    return;
                }
                
                await eventDispatcher.HandleAsync(message, cancellationToken, ea.BasicProperties.Type);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return;
            }
            
            await channel.BasicAckAsync(ea.DeliveryTag, false, ea.CancellationToken);
        };

        _ = await channel.BasicConsumeAsync(
            queue: queue,
            autoAck:false,
            consumerTag: "",
            noLocal:false,
            exclusive:false,
            consumer:consumer,
            cancellationToken: cancellationToken,
            arguments:null);
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}