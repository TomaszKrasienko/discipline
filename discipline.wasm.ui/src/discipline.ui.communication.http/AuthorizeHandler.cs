using discipline.ui.communication.http.Auth;

namespace discipline.ui.communication.http;

internal sealed class AuthorizeHandler(
    ITokenHandler tokenHandler) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await tokenHandler.GetTokenAsync(cancellationToken);
        
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Add("Authorization", $"Bearer {token}");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}