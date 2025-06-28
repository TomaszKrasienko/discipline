using System.Text;
using discipline.centre.shared.abstractions.Cache;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.application.Users.Services;
using discipline.centre.users.infrastructure.Users.RefreshToken.Configuration;
using Microsoft.Extensions.Options;

namespace discipline.centre.users.infrastructure.Users.RefreshToken;

//TODO: Name change
internal sealed class RefreshTokenSaver(
    ICacheFacade cacheFacade,
    IOptions<RefreshTokenOptions> options) : IRefreshTokenManager
{
    private readonly TimeSpan _expiry = options.Value.Expiry;
    private readonly int _refreshTokenLength = options.Value.Length;
    
    public async Task<string> GenerateAndSaveAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var refreshToken = GenerateRandom(_refreshTokenLength);
        var dto = new RefreshTokenDto(refreshToken);
        await cacheFacade.AddOrUpdateAsync(userId.Value.ToString(), dto, _expiry, cancellationToken);
        return refreshToken;
    }

    public Task<UserId?> GetUserIdAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private static string GenerateRandom(int length)
    {
        Random random = new Random();
        StringBuilder refreshTokenSb = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            refreshTokenSb.Append((char)random.Next('A', 'z'));
        }
        return refreshTokenSb.ToString();
    }
    public async Task<bool> DoesRefreshTokenExistsAsync(
        string refreshToken,
        UserId userId,
        CancellationToken cancellationToken = default)
    {
        var savedRefreshToken = await cacheFacade.GetAsync<RefreshTokenDto>(userId.ToString(), cancellationToken);

        return refreshToken == savedRefreshToken?.Value;
    }
}