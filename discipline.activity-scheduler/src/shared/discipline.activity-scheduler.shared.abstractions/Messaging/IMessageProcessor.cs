using discipline.libs.messaging.Abstractions;

namespace discipline.activity_scheduler.shared.abstractions.Messaging;

public interface IMessageProcessor
{
    Task SendAsync<TMessage>(
        TMessage message,
        CancellationToken cancellationToken = default) where TMessage : class, IMessage;
}