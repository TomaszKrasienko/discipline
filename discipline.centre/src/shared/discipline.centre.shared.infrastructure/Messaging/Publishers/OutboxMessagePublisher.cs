using discipline.centre.shared.infrastructure.Messaging.Abstractions;
using discipline.centre.shared.infrastructure.Messaging.Outbox.DAL;
using discipline.centre.shared.infrastructure.Messaging.Outbox.Models;
using discipline.libs.messaging.Abstractions;
using discipline.libs.rabbit_mq.Abstractions;
using discipline.libs.serializers.Abstractions;
using Microsoft.Extensions.Logging;

namespace discipline.centre.shared.infrastructure.Messaging.Publishers;

internal sealed class OutboxMessagePublisher(
    ILogger<OutboxMessagePublisher> logger,
    OutboxDbContext dbContext,
    ISerializer serializer,
    TimeProvider timeProvider) : IMessagePublisher
{
    public async Task PublishAsync<TMessage>(
        TMessage message,
        Ulid? messageId = null,
        CancellationToken cancellationToken = default) where TMessage : class, IMessage
    {
        try
        {
            var outboxMessage = new OutboxMessage(
                messageId ?? Ulid.NewUlid(),
                serializer.ToJson(message),
                message.GetType().FullName!,
                timeProvider.GetUtcNow(),
                null,
                0);

            await dbContext.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }
    }

    public bool IsOutbox() => true;
}