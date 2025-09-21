using discipline.activity_scheduler.account_modification.Events.External;

namespace discipline.activity_scheduler.account_modification.Strategies.Abstractions;

public interface IAccountHandlingStrategy
{
    Task HandleAsync(AccountModified @event, CancellationToken cancellationToken);
    bool CanBeApplied(string messageType);
}