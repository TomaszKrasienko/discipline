using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.users.domain.Accounts.Rules.Passwords;

internal sealed class PasswordMustBeStrongRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("Account.PasswordTooWeak");

    //TODO: Till development time
    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}