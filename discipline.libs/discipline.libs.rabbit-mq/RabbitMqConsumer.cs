using discipline.libs.exceptions.Exceptions;
using discipline.libs.messaging.Abstractions;
using discipline.libs.rabbit_mq.Conventions.Abstractions;
using discipline.libs.rabbit_mq.Factories;
using discipline.libs.rabbit_mq.Registries.Abstractions;
using discipline.libs.serializers.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace discipline.libs.rabbit_mq;

internal sealed class RabbitMqConsumer<TMessage> : IHostedService where TMessage : class, IMessage
{
    private readonly ILogger<RabbitMqConsumer<TMessage>> _logger;
    private readonly IServiceScope _scope;
    private readonly RabbitMqChannelFactory _rabbitMqChannelFactory;
    private readonly ISerializer _serializer;
    private readonly IMessagesRouteRegistry _messagesRouteRegistry;
    private readonly IConventionProvider _conventionProvider;
    private readonly Func<TMessage, CancellationToken, string?, Task> _handle;
    
    public RabbitMqConsumer(
        IServiceProvider serviceProvider,
        Func<TMessage, CancellationToken, string?, Task> handle)
    {
        _scope = serviceProvider.CreateScope();
        _logger = serviceProvider.GetRequiredService<ILogger<RabbitMqConsumer<TMessage>>>();
        _rabbitMqChannelFactory = serviceProvider.GetRequiredService<RabbitMqChannelFactory>();
        _serializer = serviceProvider.GetRequiredService<ISerializer>();
        _messagesRouteRegistry = serviceProvider.GetRequiredService<IMessagesRouteRegistry>();
        _conventionProvider = serviceProvider.GetRequiredService<IConventionProvider>();
        _handle = handle;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var channel = _rabbitMqChannelFactory.ConsumerChannel;
        var consumer = new AsyncEventingBasicConsumer(channel);

        var (exchange, routingKeys) = _messagesRouteRegistry.GetRoute<TMessage>();
        var queue = _conventionProvider.GetQueue<TMessage>();
        await InitializeTopology(channel, exchange, queue, routingKeys, cancellationToken);
        
        consumer.ReceivedAsync += async (sender, ea) =>
        {
            try
            {
                var message = _serializer.ToObject<TMessage>(ea.Body.ToArray());

                if (message is null)
                {
                    return;
                }

                await _handle(message, cancellationToken, ea.BasicProperties.Type);
            }
            catch (DisciplineNotUniqueException ex)
            {
                _logger.LogWarning("Caught not unique exception. Nacking the message without requeue");
                await channel.BasicNackAsync(ea.DeliveryTag, false, false, ea.CancellationToken);
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await channel.BasicNackAsync(ea.DeliveryTag, false, true, ea.CancellationToken);
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

    private async Task InitializeTopology(
        IChannel channel,
        string exchange,
        string queue,
        List<string> routingKeys,
        CancellationToken cancellationToken = default)
    {
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
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}