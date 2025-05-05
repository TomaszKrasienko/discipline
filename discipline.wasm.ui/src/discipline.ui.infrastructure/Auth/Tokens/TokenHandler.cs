using discipline.ui.communication.http.Auth;
using discipline.ui.communication.http.Auth.DTOs;
using discipline.ui.infrastructure.Storage.Abstractions;

namespace discipline.ui.infrastructure.Auth.Tokens;

internal sealed class TokenHandler(
    ILocalStorageAccessor localStorageAccessor) : ITokenHandler
{
    public async Task<string?> GetTokenAsync(CancellationToken cancellationToken)
    {
        var tokens = await localStorageAccessor.GetItemAsync<TokensDto>();
        return tokens?.Token;
    }

    public async Task<string> GetRefreshTokenAsync()
    {
        var tokens = await localStorageAccessor.GetItemAsync<TokensDto>();
        return tokens?.RefreshToken;
    }

    public async Task RemoveTokenAsync()
        => await localStorageAccessor.RemoveAsync<TokensDto>();

    public async Task SetAsync(TokensDto tokensDto)
    {
        await localStorageAccessor.RemoveAsync<TokensDto>();
        await localStorageAccessor.SetItemAsync(tokensDto);
    }
}