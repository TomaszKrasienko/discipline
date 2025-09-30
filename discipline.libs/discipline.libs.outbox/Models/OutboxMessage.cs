using discipline.libs.serializers.Abstractions;

namespace discipline.libs.outbox.Models;

public sealed class OutboxMessage
{
    public Ulid MessageId { get; }
    public string JsonContent { get; }
    public string MessageType { get; }
    public string Exchange { get; set; }
    public string RoutingKey { get; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset? SentAt { get; private set; }
    public int RetryCount { get; private set; }
    public string? ExceptionContent { get; private set; }

    private OutboxMessage(
        Ulid messageId,
        string jsonContent,
        string messageType,
        string exchange,
        string routingKey,
        DateTimeOffset createdAt,
        DateTimeOffset? sentAt,
        int retryCount,
        string? exceptionContent)
    {
        MessageId = messageId;
        JsonContent = jsonContent;
        MessageType = messageType;
        Exchange = exchange;
        RoutingKey = routingKey;
        CreatedAt = createdAt;
        SentAt = sentAt;
        RetryCount = retryCount;
        ExceptionContent = exceptionContent;
    }

    public static OutboxMessage Create<T>(
        Ulid messageId,
        T message,
        string exchange,
        string routingKey,
        TimeProvider timeProvider,
        ISerializer serializer)
    {
        if (message is null)
        {
            throw new ArgumentNullException(nameof(message));
        }
        
        var jsonContent = serializer.ToJson(message);
        var messageType = message.GetType().AssemblyQualifiedName;

        if (string.IsNullOrWhiteSpace(messageType))
        {
            throw new ArgumentNullException(nameof(messageType));
        }
        
        return new OutboxMessage(
            messageId,
            jsonContent,
            messageType,
            exchange,
            routingKey,
            timeProvider.GetUtcNow(),
            null,
            0,
            null);
    }
    
    internal void SetSentAt(DateTimeOffset sentAt)
        =>  SentAt = sentAt;
    
    internal void IncreaseRetryCount()
        => RetryCount++;
}