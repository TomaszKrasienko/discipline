using discipline.hangfire.account_modification.Models;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace discipline.hangfire.account_modification.DAL.Repositories;

internal interface IAccountRepository
{
    Task<bool> DoesExistAsync(
        AccountId accountId,
        CancellationToken cancellationToken);
    
    Task AddAsync(
        Account account,
        CancellationToken cancellationToken);
}

internal sealed class AccountRepository(
    AccountDbContext context) : IAccountRepository
{
    public Task<bool> DoesExistAsync(AccountId accountId, CancellationToken cancellationToken)
        => context
            .Set<Account>()
            .AnyAsync(x => x.AccountId == accountId, cancellationToken);

    public async Task AddAsync(Account account, CancellationToken cancellationToken)
    {
        context.Set<Account>().Add(account);
        await context.SaveChangesAsync(cancellationToken);
    }
}