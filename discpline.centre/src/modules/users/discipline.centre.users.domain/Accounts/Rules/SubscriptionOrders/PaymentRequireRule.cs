using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;

namespace discipline.centre.users.domain.Accounts.Rules.SubscriptionOrders;

internal sealed class PaymentRequireRule(Payment? payment, bool requirePayment) : IBusinessRule
{
    public Exception Exception => throw new DomainException("Account.SubscriptionOrder.RequirePayment");
    
    public bool IsBroken()
        => payment is null && requirePayment;
}