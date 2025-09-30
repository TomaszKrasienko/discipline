namespace discipline.libs.messaging.Abstractions;

public interface IConventionProvider
{
    (string exchange, string routingKey) GetPublisherRoutes<TMessage>(TMessage message) where TMessage : class, IMessage;
    string GetQueue<TMessage>() where TMessage : class, IMessage;
}