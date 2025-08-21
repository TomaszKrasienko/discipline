using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.domain.Subscriptions.Policies.Abstractions;

namespace discipline.centre.users.domain.Subscriptions.Policies;

internal sealed class PremiumSubscriptionPolicy : ISubscriptionPolicy
{
    public bool CanByApplied(SubscriptionType type)
        => type == SubscriptionType.Premium;
    
    public int? NumberOfRules()
        => null;

    public int? NumberOfDailyTasks()
        => null;
}