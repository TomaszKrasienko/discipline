using discipline.libs.messaging.Abstractions;
using discipline.libs.outbox.DAL;
using discipline.libs.outbox.Models;
using discipline.libs.serializers.Abstractions;
using Microsoft.Extensions.Logging;

namespace discipline.libs.outbox;

internal sealed class OutboxMessagePublisher(
    ILogger<OutboxMessagePublisher> logger,
    IConventionProvider conventionProvider,
    IOutboxMessageRepository repository,
    TimeProvider timeProvider,
    ISerializer serializer): IMessagePublisher
{
    public async Task PublishAsync<TMessage>(
        TMessage message,
        Ulid? messageId = null,
        CancellationToken cancellationToken = default) where TMessage : class, IMessage
    {
        try
        {
            var (exchange, routingKey) = conventionProvider.GetPublisherRoutes(message);

            var outboxMessage = OutboxMessage.Create(
                messageId ?? Ulid.NewUlid(),
                message,
                exchange,
                routingKey,
                timeProvider,
                serializer);
            
            await repository.AddAsync(outboxMessage, cancellationToken);
            logger.LogInformation("Saved outbox message");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Exception during saving outbox message {ex.Message}");
            throw;
        }
    }
}