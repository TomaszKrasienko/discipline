namespace discipline.hangfire.shared.abstractions.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync<TMessage>(
        TMessage message, 
        Ulid? messageId = null, 
        CancellationToken cancellationToken = default) where TMessage : class, IMessage;
}