using discipline.hangfire.shared.abstractions.Messaging;

namespace discipline.hangfire.infrastructure.Messaging.RabbitMq.Abstractions;

public interface IConventionProvider
{
    (string exchange, string routingKey) Get<TMessage>(TMessage message) where TMessage : class, IMessage;
    string GetQueue<TMessage>() where TMessage : class, IMessage;
}