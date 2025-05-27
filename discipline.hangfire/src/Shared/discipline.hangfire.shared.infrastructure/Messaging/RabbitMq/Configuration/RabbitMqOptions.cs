namespace discipline.hangfire.infrastructure.Messaging.RabbitMq.Configuration;

internal sealed record RabbitMqOptions
{
    public required string HostName { get; init; }
    public int Port { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string VirtualHost { get; init; }
    public required Dictionary<string, MessageRouteOptions> Routes { get; init; }
}

public sealed record MessageRouteOptions
{
    public required string Exchange { get; init; }
    public required string RoutingKey { get; init; }
}