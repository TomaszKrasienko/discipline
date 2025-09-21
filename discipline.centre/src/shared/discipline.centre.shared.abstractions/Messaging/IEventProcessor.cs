using discipline.libs.events.abstractions;

namespace discipline.centre.shared.abstractions.Messaging;

public interface IEventProcessor
{
    Task PublishAsync(
        CancellationToken cancellationToken,
        params IEvent[] @event);
}