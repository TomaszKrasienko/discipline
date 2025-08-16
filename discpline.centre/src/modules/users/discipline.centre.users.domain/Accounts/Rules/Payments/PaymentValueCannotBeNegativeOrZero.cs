using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.users.domain.Accounts.Rules.Payments;

internal sealed class PaymentValueCannotBeNegativeOrZero(decimal value) : IBusinessRule
{
    public Exception Exception => new DomainException("Account.SubscriptionOrder.PaymentValueBelowOrEqualZero");

    public bool IsBroken()
        => value <= 0;
}