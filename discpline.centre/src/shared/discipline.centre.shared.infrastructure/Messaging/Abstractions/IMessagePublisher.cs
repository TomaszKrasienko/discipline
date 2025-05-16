using discipline.centre.shared.abstractions.Messaging;

namespace discipline.centre.shared.infrastructure.Messaging.Abstractions;

public interface IMessagePublisher
{
    Task PublishAsync<TMessage>(TMessage message, Ulid? messageId = null, CancellationToken cancellationToken = default) where TMessage : class, IMessage;
}