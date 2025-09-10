using discipline.hangfire.account_modification.Events.External;
using discipline.hangfire.account_modification.Strategies.Abstractions;
using discipline.hangfire.domain.Accounts;
using discipline.hangfire.shared.abstractions.DAL;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.Extensions.Logging;

namespace discipline.hangfire.account_modification.Strategies;

internal sealed class AccountRegisteredStrategy(
    ILogger<AccountRegisteredStrategy> logger,
    IWriteRepository<Account, AccountId> accountRepository) : IAccountHandlingStrategy
{
    public async Task HandleAsync(AccountModified @event, CancellationToken cancellationToken)
    {
        var stronglyAccountId = AccountId.Parse(@event.AccountId);
        
        var doesAccountExist = await accountRepository
            .DoesExistAsync(x => x.AccountId == stronglyAccountId, cancellationToken);

        if (doesAccountExist)
        {
            logger.LogWarning($"Account with id {stronglyAccountId} already exists");
            return;
        }
        
        var account = Account.Create(stronglyAccountId);
        await accountRepository.AddAsync(account, cancellationToken);
    }

    public bool CanBeApplied(string messageType)
        => messageType.EndsWith("Registered");
}