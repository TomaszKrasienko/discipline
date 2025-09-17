using discipline.centre.shared.infrastructure.Messaging.Abstractions;
using discipline.libs.cqrs.Abstractions;
using discipline.libs.cqrs.abstractions.Commands;
using discipline.libs.events.abstractions;
using discipline.libs.messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.shared.infrastructure.Messaging;

internal sealed class MessageHandler<TMessage>(
    IServiceProvider serviceProvider) : IMessageHandler<TMessage> where TMessage : class, IMessage 
{
    public async Task HandleAsync(TMessage message, CancellationToken cancellationToken = default)
    {
        if (message is IEvent @event)
        {
            var scope = serviceProvider.CreateScope();
            var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
            var eventHandler = scope.ServiceProvider.GetService(handlerType);

            if (eventHandler is not null)
            {
                var method = handlerType.GetMethod("HandleAsync");
                
                if (method is null)
                {
                    return;
                }
                
                await (Task)method.Invoke(eventHandler, [@event])!;
            }
        }

        if (message is ICommand command)
        {
            var scope = serviceProvider.CreateScope();
            var dispatcher = serviceProvider.GetRequiredService<ICqrsDispatcher>();
            await dispatcher.HandleAsync(command, cancellationToken);
        }
    }
}