namespace discipline.centre.shared.infrastructure.Messaging.Publishers.Outbox.Models;

public sealed class OutboxMessage
{
    public Ulid MessageId { get; }
    public string JsonContent { get; }
    public string MessageType { get; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset? SentAt { get; }
    public int RetryCount { get; }

    //TODO: Factory method
    public OutboxMessage(
        Ulid messageId,
        string jsonContent,
        string messageType,
        DateTimeOffset createdAt,
        DateTimeOffset? sentAt,
        int retryCount)
    {
        MessageId = messageId;
        JsonContent = jsonContent;
        MessageType = messageType;
        CreatedAt = createdAt;
        SentAt = sentAt;
        RetryCount = retryCount;
    }
}