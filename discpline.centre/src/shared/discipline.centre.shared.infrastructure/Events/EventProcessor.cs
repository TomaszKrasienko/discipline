using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.infrastructure.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.shared.infrastructure.Events;

internal sealed class EventProcessor(
    IServiceProvider serviceProvider) : IEventProcessor
{
    public async Task PublishAsync(params IEvent[] domainEvents)
    {
        using var scope = serviceProvider.CreateScope();
        var messagePublishers = scope.ServiceProvider.GetRequiredService<IEnumerable<IMessagePublisher>>().ToList();
        
        foreach (var @event in domainEvents)
        {
            var sendingTasks = messagePublishers.Select(x => x.PublishAsync(@event));
            await Task.WhenAll(sendingTasks);
        }
    }
}