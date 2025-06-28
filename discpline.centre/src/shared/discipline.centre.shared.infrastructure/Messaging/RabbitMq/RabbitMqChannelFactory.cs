using discipline.centre.shared.infrastructure.Events.Brokers.RabbitMq;
using RabbitMQ.Client;

namespace discipline.centre.shared.infrastructure.Messaging.RabbitMq;

internal sealed class RabbitMqChannelFactory(RabbitMqConnectionProvider connectionProvider) : IDisposable
{
    private readonly ThreadLocal<IChannel> _consumerChannelCache = new(true);
    private readonly ThreadLocal<IChannel> _producerChannelCache = new(true);
    
    public IChannel ConsumerChannel => Create(connectionProvider.ConsumerConnection, _consumerChannelCache);
    public IChannel ProducerChannel => Create(connectionProvider.ProducerConnection, _consumerChannelCache);

    private IChannel Create(IConnection connection, ThreadLocal<IChannel> channelCache)
    {
        if (channelCache.Value is not null)
        {
            return channelCache.Value;
        }

        var channel = connection.CreateChannelAsync().GetAwaiter().GetResult();
        channelCache.Value = channel;
        return channel;
    }
    
    public void Dispose()
    {        
        foreach (var channel in _consumerChannelCache.Values)
        {
            channel.Dispose();
        }
        foreach (var channel in _producerChannelCache.Values)
        {
            channel.Dispose();
        }
        
        _consumerChannelCache.Dispose();
        _producerChannelCache.Dispose();
    }
}