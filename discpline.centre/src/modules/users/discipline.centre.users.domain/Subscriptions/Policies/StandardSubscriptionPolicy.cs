using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.domain.Subscriptions.Policies.Abstractions;

namespace discipline.centre.users.domain.Subscriptions.Policies;

internal sealed class StandardSubscriptionPolicy : ISubscriptionPolicy
{
    public bool CanByApplied(SubscriptionType type)
        => type == SubscriptionType.Standard;

    public int? NumberOfDailyTasks()
        => 10;

    public int? NumberOfRules()
        => 5;
}