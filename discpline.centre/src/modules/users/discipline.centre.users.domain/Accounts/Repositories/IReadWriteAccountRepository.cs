namespace discipline.centre.users.domain.Accounts.Repositories;

public interface IReadWriteAccountRepository : IReadAccountRepository
{
    Task AddAsync(Account account, CancellationToken cancellationToken = default);
}