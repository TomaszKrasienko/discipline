namespace discipline.hangfire.infrastructure.Configuration.Options;

internal sealed record AppOptions
{
    public required string Name { get; init; }
}