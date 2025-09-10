using discipline.libs.messaging.Abstractions;
using discipline.libs.rabbit_mq.Configurations.Options;
using discipline.libs.rabbit_mq.Registries.Abstractions;
using Microsoft.Extensions.Options;

namespace discipline.libs.rabbit_mq.Registries;

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