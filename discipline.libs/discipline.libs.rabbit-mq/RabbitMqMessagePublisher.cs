using discipline.libs.messaging.Abstractions;
using discipline.libs.rabbit_mq.Abstractions;
using discipline.libs.rabbit_mq.Conventions.Abstractions;
using discipline.libs.rabbit_mq.Factories;
using discipline.libs.serializers.Abstractions;
using RabbitMQ.Client;

namespace discipline.libs.rabbit_mq;

internal sealed class RabbitMqMessagePublisher(
    RabbitMqChannelFactory channelFactory,
    IConventionProvider conventionProvider,
    ISerializer serializer) : IMessagePublisher
{
    public async  Task PublishAsync<TMessage>(
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

        var (exchange, routingKey) = conventionProvider.GetPublisherRoutes(message);
        
        await channel.BasicPublishAsync(
            exchange: exchange,
            routingKey: routingKey,
            basicProperties: basicProperties,
            body: payload,
            mandatory:  true,
            cancellationToken: cancellationToken);
    }
}