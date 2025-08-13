using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.users.application.Accounts.Services;

public interface IRefreshTokenManager
{
    Task<string> GenerateAndSaveAsync(AccountId accountId, CancellationToken cancellationToken = default);
    Task<bool> DoesRefreshTokenExistsAsync(string refreshToken, UserId userId, CancellationToken cancellationToken = default);
}