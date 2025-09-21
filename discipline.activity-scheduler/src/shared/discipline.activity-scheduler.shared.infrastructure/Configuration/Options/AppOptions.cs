namespace discipline.activity_scheduler.shared.infrastructure.Configuration.Options;

internal sealed record AppOptions
{
    public required string Name { get; init; }
}