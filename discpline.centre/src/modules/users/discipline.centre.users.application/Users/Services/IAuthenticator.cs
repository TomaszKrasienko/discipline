using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions.Policies.Abstractions;

namespace discipline.centre.users.application.Users.Services;

public interface IAuthenticator
{
    string CreateToken(
        AccountId accountId,
        int? numberOfDailyTasks,
        int? numberOfRules);
}