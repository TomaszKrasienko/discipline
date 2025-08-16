using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.users.domain.Accounts.Rules.Intervals;

internal sealed class FinishDateBeforeStartDateRule(
    DateOnly startDate,
    DateOnly finishDate,
    string fieldName) : IBusinessRule
{
    public Exception Exception => new DomainException($"Account.SubscriptionOrder.InvalidInterval{fieldName}");

    public bool IsBroken()
        => finishDate <= startDate;
}