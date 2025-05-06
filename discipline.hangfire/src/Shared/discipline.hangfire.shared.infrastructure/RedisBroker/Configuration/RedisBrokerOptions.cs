namespace discipline.hangfire.infrastructure.RedisBroker.Configuration;

internal sealed record RedisBrokerOptions
{
    public required string ConnectionString { get; init; }
}