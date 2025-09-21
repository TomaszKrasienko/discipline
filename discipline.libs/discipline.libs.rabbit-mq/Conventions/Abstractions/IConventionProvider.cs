using discipline.libs.messaging.Abstractions;

namespace discipline.libs.rabbit_mq.Conventions.Abstractions;

internal interface IConventionProvider
{
    (string exchange, string routingKey) GetPublisherRoutes<TMessage>(TMessage message) where TMessage : class, IMessage;
    string GetQueue<TMessage>() where TMessage : class, IMessage;
}