using discipline.libs.messaging.Abstractions;

namespace discipline.centre.shared.infrastructure.Messaging.Publishers.Abstractions;

public interface IMessageProcessor
{
    Task PublishAsync<TMessage>(
        TMessage message,
        CancellationToken cancellationToken = default) where TMessage : class, IMessage;
}