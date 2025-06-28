using discipline.hangfire.infrastructure.RedisBroker;
using discipline.hangfire.shared.abstractions.Events;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class BrokerConsumerServiceRegistrationExtensions
{
    public static IServiceCollection AddBrokerConsumer<TEvent>(this IServiceCollection services)
        where TEvent : class, IEvent 
        => services
            .AddHostedService<RedisConsumer<TEvent>>();
}