using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Accounts.Specifications.SubscriptionOrder;

namespace discipline.centre.users.domain.Accounts.Services.Abstractions;

public interface IAccountService
{
    Account Create(
        AccountId accountId,
        string login,
        string password,
        SubscriptionOrderSpecification order);
}