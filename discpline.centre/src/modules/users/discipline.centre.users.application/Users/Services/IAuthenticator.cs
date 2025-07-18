using discipline.centre.users.domain.Subscriptions.Policies.Abstractions;

namespace discipline.centre.users.application.Users.Services;

public interface IAuthenticator
{
    string CreateToken(
        string userId,
        string email,
        ISubscriptionPolicy? policy);
}