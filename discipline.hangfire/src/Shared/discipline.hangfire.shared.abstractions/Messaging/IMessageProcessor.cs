using discipline.libs.messaging.Abstractions;

namespace discipline.hangfire.shared.abstractions.Messaging;

public interface IMessageProcessor
{
    Task SendAsync<TMessage>(
        TMessage message,
        CancellationToken cancellationToken = default) where TMessage : class, IMessage;
}