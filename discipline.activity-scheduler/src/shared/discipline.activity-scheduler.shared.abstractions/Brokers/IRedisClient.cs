namespace discipline.activity_scheduler.shared.abstractions.Brokers;

public interface IRedisClient
{
    Task SendAsync(string json, string route, CancellationToken cancellationToken = default);
}