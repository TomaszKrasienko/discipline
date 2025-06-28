using discipline.centre.shared.abstractions.Messaging;
using discipline.centre.shared.abstractions.Serialization;
using discipline.centre.shared.infrastructure.Events.Brokers.Internal.Channels;
using discipline.centre.shared.infrastructure.Messaging.Abstractions;

namespace discipline.centre.shared.infrastructure.Messaging.Internal;

internal sealed class InternalAsyncMessageDispatcher(
    IMessageChannel messageChannel) : IMessagePublisher
{
    public async Task PublishAsync<TMessage>(
        TMessage message,
        Ulid? messageId = null,
        CancellationToken cancellationToken = default) where TMessage : class, IMessage 
        => await messageChannel.Writer.WriteAsync(message, cancellationToken);
}