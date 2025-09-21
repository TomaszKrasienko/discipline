using discipline.centre.shared.abstractions.Messaging;
using discipline.centre.shared.infrastructure.Messaging.Publishers.Abstractions;
using discipline.libs.cqrs.abstractions.Commands;

namespace discipline.centre.shared.infrastructure.Commands;

internal sealed class CommandProcessor(
    IMessageProcessor messageProcessor) : ICommandProcessor
{
    public async Task PublishAsync(
        CancellationToken cancellationToken,
        params ICommand[] commands)
    {
        List<Task> tasks = [];

        foreach (var command in commands)
        {
            tasks.Add(messageProcessor.PublishAsync(command, cancellationToken));
        }

        await Task.WhenAll(tasks);
    }
}