using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.users.domain.Users.Rules.Users;

internal sealed class FirstNameCanNotBeEmptyRule(string firstName) : IBusinessRule
{
    public Exception Exception => new DomainException("User.FullName.EmptyFirstName");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(firstName);
}