using discipline.hangfire.infrastructure.Events.Abstractions;
using discipline.hangfire.shared.abstractions.Events;
using discipline.hangfire.shared.abstractions.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace discipline.hangfire.infrastructure.RedisBroker;

internal sealed class RedisConsumer<TEvent>(
    IConnectionMultiplexer connectionMultiplexer,
    IRouteRegister routeRegister,
    ISerializer serializer,
    IServiceProvider serviceProvider) : BackgroundService where TEvent : class, IEvent  
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var subscriber = connectionMultiplexer.GetSubscriber();
        var channel = routeRegister.GetChannel<TEvent>();

        await subscriber.SubscribeAsync(new RedisChannel(channel, RedisChannel.PatternMode.Auto), async (channel, message) =>
        {
            using var scope = serviceProvider.CreateScope();
            var eventDispatcher = scope.ServiceProvider.GetRequiredService<IEventDispatcher>();

            var @event = serializer.ToObject<TEvent>(message);
            await eventDispatcher.HandleAsync(@event, stoppingToken);
        });
    }
}