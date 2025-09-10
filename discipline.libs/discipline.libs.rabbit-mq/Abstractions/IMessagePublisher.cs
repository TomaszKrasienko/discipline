using discipline.libs.messaging.Abstractions;

namespace discipline.libs.rabbit_mq.Abstractions;

public interface IMessagePublisher
{
    Task PublishAsync<TMessage>(
        TMessage message, 
        Ulid? messageId = null, 
        CancellationToken cancellationToken = default) where TMessage : class, IMessage;
}