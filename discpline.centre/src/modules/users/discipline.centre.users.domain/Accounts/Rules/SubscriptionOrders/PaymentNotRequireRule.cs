using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;

namespace discipline.centre.users.domain.Accounts.Rules.SubscriptionOrders;

internal sealed class PaymentNotRequireRule(Payment? payment, bool requirePayment) : IBusinessRule
{
    public Exception Exception => new DomainException("Account.SubscriptionOrder.NotRequirePayment");

    public bool IsBroken()
        => payment is not null && !requirePayment;
}