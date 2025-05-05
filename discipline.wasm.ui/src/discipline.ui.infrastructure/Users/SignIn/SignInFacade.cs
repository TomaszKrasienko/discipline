using System.Net.Http.Json;
using discipline.ui.communication.http.Auth;
using discipline.ui.communication.http.Auth.DTOs;
using discipline.ui.communication.http.Users;
using discipline.ui.communication.http.Users.Requests;
using OneOf;
using Refit;

namespace discipline.ui.infrastructure.Users.SignIn;

internal sealed class SignInFacade(
    IUserHttpClient userHttpClient,
    ITokenHandler tokenHandler) : ISignInFacade
{
    public async Task<OneOf<bool, string>> HandleAsync(string email, string password, CancellationToken cancellationToken)
    {
        var signInRequest = new SignInRequestDto(email, password);

        HttpResponseMessage signInResponse;
        try
        {
            signInResponse = await userHttpClient.SignIn(signInRequest, cancellationToken);

            if (!signInResponse.IsSuccessStatusCode)
            {
                var response = await signInResponse.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken);
                return response?.Detail ?? "Unknown Error";
            }
        }
        catch (Exception )
        {
            return "Server communication error";
        }

        var tokens = await signInResponse.Content.ReadFromJsonAsync<TokensDto>(cancellationToken);
        await tokenHandler.SetAsync(tokens!);
        return true;
    }
}