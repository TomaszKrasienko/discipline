using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.domain.Subscriptions.Policies.Abstractions;

namespace discipline.centre.users.domain.Subscriptions.Policies.PaidSubscriptions;

internal sealed class PremiumSubscriptionPolicy : ISubscriptionPolicy
{
    public int? NumberOfRules()
        => null;

    public int? NumberOfDailyTasks()
        => null;

    public bool CanByApplied(SubscriptionType type)
        => type == SubscriptionType.Premium;
}