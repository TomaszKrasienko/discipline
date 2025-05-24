using discipline.hangfire.shared.abstractions.Messaging;

namespace discipline.hangfire.infrastructure.Messaging.Abstractions;

internal interface IMessagesRouteRegistry
{
    (string exchange, string routingKey) GetRoute<TMessage>() where TMessage : class, IMessage;
}