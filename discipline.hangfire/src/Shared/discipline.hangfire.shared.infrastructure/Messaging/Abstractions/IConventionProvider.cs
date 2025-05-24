using discipline.hangfire.shared.abstractions.Messaging;

namespace discipline.hangfire.infrastructure.Messaging.Abstractions;

public interface IConventionProvider
{
    string GetQueue<TMessage>() where TMessage : class, IMessage;
}