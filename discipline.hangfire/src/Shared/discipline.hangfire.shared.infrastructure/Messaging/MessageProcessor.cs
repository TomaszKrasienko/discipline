using discipline.hangfire.shared.abstractions.Messaging;
using discipline.libs.messaging.Abstractions;
using discipline.libs.rabbit_mq.Abstractions;

namespace discipline.hangfire.infrastructure.Messaging;

internal sealed class MessageProcessor(
    IMessagePublisher publisher) : IMessageProcessor
{
    public async Task SendAsync<TMessage>(
        TMessage message,
        CancellationToken cancellationToken = default) where TMessage : class, IMessage
    {
        await publisher.PublishAsync(
            message,
            null,
            cancellationToken);
    }
}