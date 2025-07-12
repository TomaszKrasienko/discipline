using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.users.domain.Users.Rules.Users;

internal sealed class FirstNameMustBeFrom2To100LengthRule(string firstName) : IBusinessRule
{
    public Exception Exception => new DomainException("User.FullName.FirstNameInvalidLength");

    public bool IsBroken()
        => firstName.Length is < 2 or > 100;
}