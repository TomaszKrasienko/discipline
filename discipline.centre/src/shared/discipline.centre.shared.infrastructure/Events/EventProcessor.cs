using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.infrastructure.Messaging.Publishers.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.shared.infrastructure.Events;

internal sealed class EventProcessor(
    IServiceProvider serviceProvider) : IEventProcessor
{
    public async Task PublishAsync(
        CancellationToken cancellationToken = default,
        params IEvent[] events)
    {
        using var scope = serviceProvider.CreateScope();
        var messageProcessor = scope.ServiceProvider.GetRequiredService<IMessageProcessor>();
        List<Task> tasks = new List<Task>();
        
        foreach (var @event in events)
        {
            tasks.Add(messageProcessor.PublishAsync(@event, cancellationToken));
        }
        
        await Task.WhenAll(tasks);
    }
}