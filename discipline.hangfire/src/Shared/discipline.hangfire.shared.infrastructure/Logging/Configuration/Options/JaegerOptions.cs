namespace discipline.hangfire.infrastructure.Logging.Configuration.Options;

internal sealed record JaegerOptions
{
    public string Endpoint { get; init; } = string.Empty;
}