using discipline.centre.shared.abstractions.Messaging;
using discipline.centre.shared.abstractions.Serialization;
using discipline.centre.shared.infrastructure.Messaging.Abstractions;
using discipline.centre.shared.infrastructure.Messaging.RabbitMq.Abstractions;
using RabbitMQ.Client;

namespace discipline.centre.shared.infrastructure.Messaging.RabbitMq;

internal sealed class RabbitMqMessagePublisher(
    RabbitMqChannelFactory channelFactory,
    IMessageConventionProvider conventionProvider,
    ISerializer serializer) : IMessagePublisher
{
    public async Task PublishAsync<TMessage>(
        TMessage message, 
        Ulid? messageId = null, 
        CancellationToken cancellationToken = default) where TMessage : class, IMessage
    {
        var channel = channelFactory.ProducerChannel;
        var payload = serializer.ToByteJson(message);

        var basicProperties = new BasicProperties
        {
            MessageId = messageId.ToString(),
            Type = message.GetType().Name,
        };

        var (exchange, routingKey) = conventionProvider.Get(message);
        
        await channel.BasicPublishAsync(
            exchange: exchange,
            routingKey: routingKey,
            basicProperties: basicProperties,
            body: payload,
            mandatory:  true,
            cancellationToken: cancellationToken);
    }
}