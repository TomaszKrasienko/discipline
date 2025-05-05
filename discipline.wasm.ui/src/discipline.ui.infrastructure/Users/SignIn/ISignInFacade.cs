using OneOf;

namespace discipline.ui.infrastructure.Users.SignIn;

public interface ISignInFacade
{
    Task<OneOf<bool, string>> HandleAsync(string email, string password, CancellationToken cancellationToken);
}