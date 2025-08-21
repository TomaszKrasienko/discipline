using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Accounts.Services.Abstractions;
using discipline.centre.users.domain.Accounts.Specifications;
using discipline.centre.users.domain.Accounts.Specifications.Account;
using discipline.centre.users.domain.Accounts.Specifications.SubscriptionOrder;

namespace discipline.centre.users.domain.Accounts.Services;

internal sealed class AccountService(
    IPasswordManager passwordManager,
    TimeProvider timeProvider) : IAccountService
{
    public Account Create(
        AccountId accountId,
        string login,
        string password,
        SubscriptionOrderSpecification order)
    {
        var hashedPassword = passwordManager.Secure(password);
        var passwordSpecification = new PasswordSpecification(password, hashedPassword);
        
        return Account.Create(
            accountId,
            login,
            passwordSpecification,
            timeProvider,
            order);
    }
}