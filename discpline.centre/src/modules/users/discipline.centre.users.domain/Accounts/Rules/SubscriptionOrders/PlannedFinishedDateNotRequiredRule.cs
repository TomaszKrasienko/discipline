using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;

namespace discipline.centre.users.domain.Accounts.Rules.SubscriptionOrders;

internal sealed class PlannedFinishedDateNotRequiredRule(
    Interval interval, 
    bool requirePayment) : IBusinessRule
{
    public Exception Exception => throw new DomainException("Account.SubscriptionOrder.NotRequiredPlannedFinishDate");
    public bool IsBroken()
        => interval.PlannedFinishDate is not null
           && !requirePayment;
}