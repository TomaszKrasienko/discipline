using discipline.hangfire.create_activity_from_planned.Publishers.Abstractions;
using discipline.hangfire.shared.abstractions.Brokers;
using discipline.hangfire.shared.abstractions.Serializer;

namespace discipline.hangfire.create_activity_from_planned.Publishers;

internal sealed class RedisBrokerPublisher(
    ISerializer serializer,
    IRedisClient redisClient) : IBrokerPublisher
{
    public async Task SendAsync<T>(T message, CancellationToken cancellationToken) where T : class
    {
        var json = serializer.ToJson(message);
        
        await redisClient.SendAsync(json, message.GetType().Name, cancellationToken);
    }
}