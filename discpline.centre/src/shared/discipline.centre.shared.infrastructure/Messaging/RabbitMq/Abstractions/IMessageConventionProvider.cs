using discipline.centre.shared.abstractions.Messaging;
using discipline.centre.shared.infrastructure.Messaging.Abstractions;

namespace discipline.centre.shared.infrastructure.Messaging.RabbitMq.Abstractions;

public interface IMessageConventionProvider
{
    (string exchange, string routingKey) Get<TMessage>(TMessage message) where TMessage : class, IMessage;
}