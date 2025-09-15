using discipline.activity_scheduler.account_modification.Events.External;
using discipline.activity_scheduler.account_modification.Strategies.Abstractions;

namespace discipline.activity_scheduler.account_modification.Strategies;

internal sealed class AccountDeletedStrategy : IAccountHandlingStrategy
{
    public Task HandleAsync(AccountModified @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public bool CanBeApplied(string messageType)
        => false;
}