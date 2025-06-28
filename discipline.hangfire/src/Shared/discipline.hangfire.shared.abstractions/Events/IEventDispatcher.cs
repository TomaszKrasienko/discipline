namespace discipline.hangfire.shared.abstractions.Events;

public interface IEventDispatcher
{
    Task HandleAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : class, IEvent;
}