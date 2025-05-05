using discipline.ui.communication.http.Auth.DTOs;

namespace discipline.ui.communication.http.Auth;

public interface ITokenHandler
{
    Task<string?> GetTokenAsync(CancellationToken cancellationToken);
    Task<string> GetRefreshTokenAsync();
    Task RemoveTokenAsync();
    Task SetAsync(TokensDto tokensDto);
}