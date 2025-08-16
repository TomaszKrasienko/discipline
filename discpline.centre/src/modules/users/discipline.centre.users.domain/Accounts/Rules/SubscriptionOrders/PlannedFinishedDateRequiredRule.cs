using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;

namespace discipline.centre.users.domain.Accounts.Rules.SubscriptionOrders;

internal sealed class PlannedFinishedDateRequiredRule(
    Interval interval, 
    bool requirePayment) : IBusinessRule
{
    public Exception Exception => throw new DomainException("Account.SubscriptionOrder.RequiredPlannedFinishDate");
    public bool IsBroken()
        => interval.PlannedFinishDate is null
        && requirePayment;
}