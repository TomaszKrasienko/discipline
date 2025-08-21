using discipline.centre.shared.abstractions.Messaging;

namespace discipline.centre.shared.infrastructure.Messaging.RabbitMq.Abstractions;

public interface IMessageConventionProvider
{
    (string exchange, string routingKey) Get<TMessage>(TMessage message) where TMessage : class, IMessage;
    string GetQueue<TMessage>() where TMessage : class, IMessage;
}