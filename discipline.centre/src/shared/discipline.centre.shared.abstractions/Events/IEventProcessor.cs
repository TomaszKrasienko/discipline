namespace discipline.centre.shared.abstractions.Events;

public interface IEventProcessor
{
    Task PublishAsync(
        CancellationToken cancellationToken = default,
        params IEvent[] events);
}