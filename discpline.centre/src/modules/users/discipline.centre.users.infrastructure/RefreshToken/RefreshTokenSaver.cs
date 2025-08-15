using System.Text;
using discipline.centre.shared.abstractions.Cache;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Accounts.Services;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.infrastructure.Accounts.RefreshToken.Configuration;
using Microsoft.Extensions.Options;

namespace discipline.centre.users.infrastructure.RefreshToken;

//TODO: Name change
internal sealed class RefreshTokenSaver(
    ICacheFacade cacheFacade,
    IOptions<RefreshTokenOptions> options) : IRefreshTokenManager
{
    private readonly TimeSpan _expiry = options.Value.Expiry;
    private readonly int _refreshTokenLength = options.Value.Length;

    public async Task<string> GenerateAndReplaceAsync(
        AccountId accountId,
        CancellationToken cancellationToken = default)
    {
        var refreshToken = GenerateRandom(_refreshTokenLength);
        var dto = new RefreshTokenDto(refreshToken);
        await cacheFacade.AddOrUpdateAsync(accountId.ToString(), dto, _expiry, cancellationToken);
        return refreshToken;
    }

    public async Task<bool> DoesRefreshTokenExistAsync(
        string refreshToken,
        AccountId accountId,
        CancellationToken cancellationToken = default)
    {
        var savedRefreshToken = await cacheFacade.GetAsync<RefreshTokenDto>(
            accountId.ToString(),
            cancellationToken);

        return refreshToken == savedRefreshToken?.Value;
    }

    private static string GenerateRandom(int length)
    {
        Random random = new Random();
        StringBuilder refreshTokenSb = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            refreshTokenSb.Append((char)random.Next('a', 'z'));
        }
        return refreshTokenSb.ToString();
    }
}