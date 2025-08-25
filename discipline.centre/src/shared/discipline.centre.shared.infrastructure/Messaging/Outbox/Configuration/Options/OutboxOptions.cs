namespace discipline.centre.shared.infrastructure.Messaging.Outbox.Configuration.Options;

public record OutboxOptions
{
    public bool IsEnabled { get; init; }
    public string? ConnectionString { get; init; }
}