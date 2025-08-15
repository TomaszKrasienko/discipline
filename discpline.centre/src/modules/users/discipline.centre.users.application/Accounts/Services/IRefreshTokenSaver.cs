using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.users.application.Accounts.Services;

public interface IRefreshTokenManager
{
    Task<string> GenerateAndReplaceAsync(
        AccountId accountId,
        CancellationToken cancellationToken = default);
    
    Task<bool> DoesRefreshTokenExistAsync(
        string refreshToken,
        AccountId accountId,
        CancellationToken cancellationToken = default);
}