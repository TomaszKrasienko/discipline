using discipline.libs.messaging.Abstractions;

namespace discipline.centre.shared.infrastructure.Messaging.RabbitMq.Abstractions;

internal interface IMessagesRouteRegistry
{
    (string exchange, string routingKey) GetRoute<TMessage>() where TMessage : class, IMessage;
}