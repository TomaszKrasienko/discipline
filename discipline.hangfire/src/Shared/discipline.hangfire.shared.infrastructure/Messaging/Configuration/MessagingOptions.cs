namespace discipline.hangfire.infrastructure.Messaging.Configuration;

public sealed record MessagingOptions
{
    public required Dictionary<string, MessageRouteOptions> Routes { get; init; }
}

public sealed record MessageRouteOptions
{
    public required string Exchange { get; init; }
    public required string RoutingKey { get; init; }
}