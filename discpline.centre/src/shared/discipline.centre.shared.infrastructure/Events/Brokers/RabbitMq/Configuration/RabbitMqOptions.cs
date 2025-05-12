namespace discipline.centre.shared.infrastructure.Events.Brokers.RabbitMq.Configuration;

internal sealed record RabbitMqOptions
{
    public required string HostName { get; init; }
    public int Port { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string VirtualHost { get; init; }
}