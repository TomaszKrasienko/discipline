using discipline.centre.shared.abstractions.Messaging;
using discipline.centre.shared.abstractions.Serialization;
using discipline.centre.shared.infrastructure.Messaging.Abstractions;
using discipline.centre.shared.infrastructure.Messaging.RabbitMq.Abstractions;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace discipline.centre.shared.infrastructure.Messaging.RabbitMq;

internal sealed class RabbitMqConsumer<TMessage>(
    RabbitMqChannelFactory rabbitMqChannelFactory,
    ISerializer serializer,
    IMessagesRouteRegistry routeRegistry,
    IMessageConventionProvider conventionProvider,
    IMessageHandler<TMessage> handler) : IHostedService where TMessage : class, IMessage
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var channel = rabbitMqChannelFactory.ConsumerChannel;
        var consumer = new AsyncEventingBasicConsumer(channel);

        var (exchange, routingKey) = routeRegistry.GetRoute<TMessage>();
        var queue = conventionProvider.GetQueue<TMessage>();

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
        
        await channel.QueueBindAsync(
            queue: queue,
            exchange: exchange,
            routingKey: routingKey,
            cancellationToken: cancellationToken);

        consumer.ReceivedAsync += async (sender, ea) =>
        {
            try
            {
                var message = serializer.ToObject<TMessage>(ea.Body.ToArray());

                if (message is null)
                {
                    return;
                }
                
                await handler.HandleAsync(message, cancellationToken);
            }
            catch (Exception)
            {
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