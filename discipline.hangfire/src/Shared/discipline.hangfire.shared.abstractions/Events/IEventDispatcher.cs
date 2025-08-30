namespace discipline.hangfire.shared.abstractions.Events;

public interface IEventDispatcher
{
    Task HandleAsync<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken,
        string? messageType = null) where TEvent : class, IEvent;
}