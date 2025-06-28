using RabbitMQ.Client;

namespace discipline.centre.shared.infrastructure.Events.Brokers.RabbitMq;

internal sealed class RabbitMqConnectionProvider(IConnection consumerConnection, IConnection producerConnection)
{
    public IConnection ConsumerConnection { get; } = consumerConnection;
    public IConnection ProducerConnection { get; } = producerConnection;
}