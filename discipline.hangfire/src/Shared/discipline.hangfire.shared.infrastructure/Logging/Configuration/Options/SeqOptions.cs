namespace discipline.hangfire.infrastructure.Logging.Configuration.Options;

internal sealed record SeqOptions
{
    public required string Url { get; init; }
}