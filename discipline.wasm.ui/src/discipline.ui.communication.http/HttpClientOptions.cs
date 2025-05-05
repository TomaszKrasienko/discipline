namespace discipline.ui.communication.http;

public sealed record HttpClientOptions
{
    public required string Url { get; init; }
    public TimeSpan Timeout { get; init; }
};