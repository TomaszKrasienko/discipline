using discipline.hangfire.shared.abstractions.Brokers;
using StackExchange.Redis;

namespace discipline.hangfire.infrastructure.RedisBroker;

internal sealed class RedisClient(
    IConnectionMultiplexer connectionMultiplexer) : IRedisClient
{
    private readonly ISubscriber _subscriber = connectionMultiplexer.GetSubscriber();
    
    public Task SendAsync(string json, string route, CancellationToken cancellationToken = default)
        => _subscriber.PublishAsync(new RedisChannel(route, RedisChannel.PatternMode.Auto), json);    
}