namespace discipline.centre.users.domain.Accounts.Repositories;

public interface IReadAccountRepository
{
    Task<bool> DoesEmailExistAsync(string email, CancellationToken cancellationToken = default);
    Task<Account?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}