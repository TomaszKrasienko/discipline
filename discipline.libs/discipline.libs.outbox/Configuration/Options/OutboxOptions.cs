namespace discipline.libs.outbox.Configuration.Options;

public sealed record OutboxOptions
{
    public bool IsEnabled  { get; init; }
    public string? ConnectionString { get; init; }
}