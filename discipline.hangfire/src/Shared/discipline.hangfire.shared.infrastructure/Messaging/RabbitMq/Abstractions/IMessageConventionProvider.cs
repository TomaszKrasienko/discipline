using discipline.hangfire.shared.abstractions.Messaging;

namespace discipline.hangfire.infrastructure.Messaging.RabbitMq.Abstractions;

public interface IMessageConventionProvider
{
    (string exchange, string routingKey) Get<TMessage>(TMessage message) where TMessage : class, IMessage;
}