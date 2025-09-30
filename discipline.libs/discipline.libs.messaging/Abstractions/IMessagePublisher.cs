namespace discipline.libs.messaging.Abstractions;

public interface IMessagePublisher
{
    Task PublishAsync<TMessage>(
        TMessage message, 
        Ulid? messageId = null, 
        CancellationToken cancellationToken = default) where TMessage : class, IMessage;
}