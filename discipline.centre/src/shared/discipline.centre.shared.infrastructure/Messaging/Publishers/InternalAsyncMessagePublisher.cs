using discipline.centre.shared.infrastructure.Messaging.Abstractions;
using discipline.centre.shared.infrastructure.Messaging.Internal.Channels;
using discipline.libs.messaging.Abstractions;

namespace discipline.centre.shared.infrastructure.Messaging.Publishers;

internal sealed class InternalAsyncMessagePublisher(
    IMessageChannel messageChannel) : IMessagePublisher
{
    public async Task PublishAsync<TMessage>(
        TMessage message,
        Ulid? messageId = null,
        CancellationToken cancellationToken = default) where TMessage : class, IMessage 
        => await messageChannel.Writer.WriteAsync(message, cancellationToken);

    public bool IsOutbox() => false;
}