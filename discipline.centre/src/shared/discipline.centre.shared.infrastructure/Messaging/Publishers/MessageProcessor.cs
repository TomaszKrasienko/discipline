using discipline.centre.shared.abstractions.Messaging;
using discipline.centre.shared.infrastructure.Messaging.Abstractions;
using discipline.centre.shared.infrastructure.Messaging.Outbox.Configuration.Options;
using discipline.centre.shared.infrastructure.Messaging.Publishers.Abstractions;
using Microsoft.Extensions.Options;

namespace discipline.centre.shared.infrastructure.Messaging.Publishers;

internal sealed class MessageProcessor(
    IOptions<OutboxOptions> outboxOptions,
    IEnumerable<IMessagePublisher> publishers) : IMessageProcessor
{
    public async Task PublishAsync<TMessage>(
        TMessage message,
        CancellationToken cancellationToken = default) where TMessage : class, IMessage
    {
        if (outboxOptions.Value.IsEnabled)
        {
            var outboxPublisher = publishers.Single(x => x.IsOutbox());
            
            await outboxPublisher.PublishAsync(
                message,
                null,
                cancellationToken); 
        }
        else
        {
            var directPublishers = publishers
                .Where(x => !x.IsOutbox())
                .ToList();
            
            var tasks = directPublishers
                .Select(x => x.PublishAsync(message, Ulid.NewUlid(), cancellationToken))
                .ToList();
            
            await Task.WhenAll(tasks);
        }
    }
}