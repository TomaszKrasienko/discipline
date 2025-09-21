using discipline.libs.events.abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace discipline.libs.events;

internal sealed class EventDispatcher(IServiceProvider serviceProvider,
    ILogger<EventDispatcher> logger) : IEventDispatcher
{
    public async Task HandleAsync<TEvent>(TEvent @event, CancellationToken cancellationToken, string? messageType = null) where TEvent : class, IEvent
    {
        using var scope = serviceProvider.CreateScope();
        var handlers = scope.ServiceProvider.GetRequiredService<IEnumerable<IEventHandler<TEvent>>>();

        logger.LogInformation("Publishing {0}", typeof(TEvent).Name);
        
        var tasks = handlers.Select(x => x.HandleAsync(@event, CancellationToken.None));
        await Task.WhenAll(tasks);
    }
}