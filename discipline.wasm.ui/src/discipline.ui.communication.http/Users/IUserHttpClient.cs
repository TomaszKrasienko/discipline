using discipline.ui.communication.http.Users.Requests;
using Refit;

namespace discipline.ui.communication.http.Users;

public interface IUserHttpClient
{
    [Post("/api/users-module/users/tokens")]
    Task<HttpResponseMessage> SignIn(SignInRequestDto signInRequestDto, CancellationToken cancellationToken);
}