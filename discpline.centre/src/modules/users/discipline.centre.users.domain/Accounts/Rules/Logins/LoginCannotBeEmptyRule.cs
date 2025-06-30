using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.users.domain.Accounts.Rules.Logins;

internal sealed class LoginCannotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("Account.EmptyLogin");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}