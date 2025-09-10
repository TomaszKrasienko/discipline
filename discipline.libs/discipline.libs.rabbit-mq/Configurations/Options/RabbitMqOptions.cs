namespace discipline.libs.rabbit_mq.Configurations.Options;

internal sealed record RabbitMqOptions
{
    public required string HostName { get; init; }
    public int Port { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string VirtualHost { get; init; }
    public required Dictionary<string, MessageRouteOptions> Routes { get; init; }
}