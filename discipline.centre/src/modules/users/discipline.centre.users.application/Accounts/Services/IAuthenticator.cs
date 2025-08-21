using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.users.application.Accounts.Services;

public interface IAuthenticator
{
    string CreateToken(
        AccountId accountId,
        bool hasPayedSubscription,
        DateOnly? activeTill,
        int? numberOfDailyTasks,
        int? numberOfRules);
}