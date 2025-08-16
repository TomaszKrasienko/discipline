using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.users.domain.Users.Rules.Users;

internal sealed class LastNameCanNotBeEmptyRule(string lastName) : IBusinessRule
{
    public Exception Exception => new DomainException("User.FullName.EmptyLastName");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(lastName);
}