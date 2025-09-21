using discipline.libs.messaging.Abstractions;

namespace discipline.libs.rabbit_mq.Registries.Abstractions;

internal interface IMessagesRouteRegistry
{
    (string exchange, List<string> routingKeys) GetRoute<TMessage>() where TMessage : class, IMessage;
}