namespace discipline.centre.users.domain.Accounts.Repositories;

public interface IReadAccountRepository
{
    Task<bool> DoesLoginExistAsync(string login, CancellationToken cancellationToken = default);
    Task<Account?> GetByLoginAsync(string login, CancellationToken cancellationToken = default);
}