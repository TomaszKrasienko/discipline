using discipline.hangfire.account_modification.Events.External;
using discipline.hangfire.account_modification.Strategies.Abstractions;

namespace discipline.hangfire.account_modification.Strategies;

internal sealed class AccountDeletedStrategy : IAccountHandlingStrategy
{
    public Task HandleAsync(AccountModified @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public bool CanBeApplied(string messageType)
        => false;
}