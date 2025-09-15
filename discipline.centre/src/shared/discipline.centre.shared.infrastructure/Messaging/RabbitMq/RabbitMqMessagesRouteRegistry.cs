using discipline.centre.shared.infrastructure.Messaging.RabbitMq.Abstractions;
using discipline.centre.shared.infrastructure.Messaging.RabbitMq.Configuration;
using discipline.libs.messaging.Abstractions;
using Microsoft.Extensions.Options;

namespace discipline.centre.shared.infrastructure.Messaging.RabbitMq;

internal sealed class RabbitMqMessagesRouteRegistry(
    IOptions<RabbitMqOptions> options) : IMessagesRouteRegistry
{
    private readonly Dictionary<string,MessageRouteOptions> _options = options.Value.Routes;
    
    public (string exchange, string routingKey) GetRoute<TMessage>() where TMessage : class, IMessage
    {
        var typeName = typeof(TMessage).Name;

        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        if (_options?.TryGetValue(typeName, out var route) ?? false)
        {
            return (route.Exchange, route.RoutingKey);
        }
        
        throw new ArgumentException($"{typeName} is not registered");
    }
}