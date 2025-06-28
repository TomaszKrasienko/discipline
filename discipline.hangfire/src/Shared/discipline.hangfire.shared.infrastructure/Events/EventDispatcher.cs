using discipline.hangfire.shared.abstractions.Events;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.hangfire.infrastructure.Events;

internal sealed class EventDispatcher(
    IServiceProvider serviceProvider) : IEventDispatcher
{
    public async Task HandleAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : class, IEvent
    {
        using var scope = serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<TEvent>>();
        await handler.HandleAsync(@event, cancellationToken);
    }
}