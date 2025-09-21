namespace discipline.activity_scheduler.shared.infrastructure.Logging.Configuration.Options;

internal sealed record JaegerOptions
{
    public string Endpoint { get; init; } = string.Empty;
}