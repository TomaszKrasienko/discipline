using discipline.centre.shared.abstractions.Messaging;

namespace discipline.centre.shared.infrastructure.Messaging.Abstractions;

public interface IMessageHandler<TMessage> where TMessage : class, IMessage
{
    Task HandleAsync(TMessage message, CancellationToken cancellationToken = default);
}