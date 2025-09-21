using discipline.libs.cqrs.abstractions.Commands;

namespace discipline.centre.shared.abstractions.Messaging;

public interface ICommandProcessor
{
    Task PublishAsync(
        CancellationToken cancellationToken,
        params ICommand[] commands);
}