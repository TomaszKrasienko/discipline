using RabbitMQ.Client;

namespace discipline.libs.rabbit_mq.Connections;

internal sealed class RabbitMqConnectionProvider(IConnection consumerConnection, IConnection producerConnection)
{
    public IConnection ConsumerConnection { get; } = consumerConnection;
    public IConnection ProducerConnection { get; } = producerConnection;
}