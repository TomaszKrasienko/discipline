namespace discipline.libs.events.abstractions;

public interface IEventDispatcher
{
    Task HandleAsync<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken,
        string? messageType = null) where TEvent : class, IEvent;
}