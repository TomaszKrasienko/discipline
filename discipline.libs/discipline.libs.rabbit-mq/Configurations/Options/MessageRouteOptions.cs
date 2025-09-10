namespace discipline.libs.rabbit_mq.Configurations.Options;

public sealed record MessageRouteOptions
{
    public required string Exchange { get; init; }
    public required string[] RoutingKeys { get; init; }
}