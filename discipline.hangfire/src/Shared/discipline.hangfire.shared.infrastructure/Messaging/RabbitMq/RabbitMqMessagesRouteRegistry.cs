using discipline.hangfire.infrastructure.Messaging.RabbitMq.Abstractions;
using discipline.hangfire.infrastructure.Messaging.RabbitMq.Configuration;
using discipline.hangfire.shared.abstractions.Messaging;
using Microsoft.Extensions.Options;

namespace discipline.hangfire.infrastructure.Messaging.RabbitMq;

internal sealed class RabbitMqMessagesRouteRegistry(
    IOptions<RabbitMqOptions> options) : IMessagesRouteRegistry
{
    private readonly Dictionary<string,MessageRouteOptions> _options = options.Value.Routes;
    
    public (string exchange, List<string> routingKeys) GetRoute<TMessage>() where TMessage : class, IMessage
    {
        var typeName = typeof(TMessage).Name;

        if (_options.TryGetValue(typeName, out var route))
        {
            return (route.Exchange, route.RoutingKeys.ToList());
        }
        
        throw new ArgumentException($"{typeName} is not registered");
    }
}