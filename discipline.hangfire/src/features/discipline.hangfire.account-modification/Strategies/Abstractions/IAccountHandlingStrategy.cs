using discipline.hangfire.account_modification.Events.External;

namespace discipline.hangfire.account_modification.Strategies.Abstractions;

public interface IAccountHandlingStrategy
{
    Task HandleAsync(AccountModified @event, CancellationToken cancellationToken);
    bool CanBeApplied(string messageType);
}