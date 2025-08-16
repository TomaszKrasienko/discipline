using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.DAL;
using discipline.centre.users.domain.Accounts;
using discipline.centre.users.domain.Accounts.Repositories;
using discipline.centre.users.infrastructure.DAL.Accounts.Documents;

namespace discipline.centre.users.infrastructure.DAL.Accounts.Repositories;

internal sealed class MongoAccountRepository(
    UsersMongoContext context) : BaseRepository<AccountDocument>(context), IReadWriteAccountRepository
{
    public Task<bool> DoesLoginExistAsync(
        string login,
        CancellationToken cancellationToken = default)
        => AnyAsync(x => x.Login == login, cancellationToken);

    public async Task<Account?> GetByLoginAsync(string login, CancellationToken cancellationToken = default)
        => (await GetAsync(x => x.Login == login, cancellationToken))?.ToEntity();

    public async Task<Account?> GetByIdAsync(AccountId accountId, CancellationToken cancellationToken = default)
        => (await GetAsync(x => x.Id == accountId.ToString(), cancellationToken))?.ToEntity();

    public async Task AddAsync(Account account, CancellationToken cancellationToken = default)
    {
        var document = account.ToDocument();
        await AddAsync(document, cancellationToken);
    }
}