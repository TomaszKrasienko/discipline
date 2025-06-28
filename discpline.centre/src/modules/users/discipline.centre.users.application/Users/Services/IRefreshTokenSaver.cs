using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.users.application.Users.Services;

public interface IRefreshTokenManager
{
    Task<string> GenerateAndSaveAsync(UserId userId, CancellationToken cancellationToken = default);
    Task<bool> DoesRefreshTokenExistsAsync(string refreshToken, UserId userId, CancellationToken cancellationToken = default);
}