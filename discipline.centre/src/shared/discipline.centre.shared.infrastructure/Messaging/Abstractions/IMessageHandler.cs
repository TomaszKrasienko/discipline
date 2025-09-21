using discipline.libs.messaging.Abstractions;

namespace discipline.centre.shared.infrastructure.Messaging.Abstractions;

public interface IMessageHandler<TMessage> where TMessage : class, IMessage
{
    Task HandleAsync(TMessage message, CancellationToken cancellationToken = default);
}