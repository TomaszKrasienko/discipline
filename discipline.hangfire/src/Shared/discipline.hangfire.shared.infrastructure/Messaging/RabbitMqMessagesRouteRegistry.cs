using discipline.hangfire.infrastructure.Messaging.Abstractions;
using discipline.hangfire.infrastructure.Messaging.Configuration;
using discipline.hangfire.shared.abstractions.Messaging;
using Microsoft.Extensions.Options;

namespace discipline.hangfire.infrastructure.Messaging;

internal sealed class RabbitMqMessagesRouteRegistry(
    IOptions<MessagingOptions> options) : IMessagesRouteRegistry
{
    private readonly MessagingOptions _options = options.Value;
    
    public (string exchange, string routingKey) GetRoute<TMessage>() where TMessage : class, IMessage
    {
        var typeName = typeof(TMessage).Name;

        if (_options.Routes.TryGetValue(typeName, out var route))
        {
            return (route.Exchange, route.RoutingKey);
        }
        
        throw new ArgumentException($"{typeName} is not registered");
    }
}