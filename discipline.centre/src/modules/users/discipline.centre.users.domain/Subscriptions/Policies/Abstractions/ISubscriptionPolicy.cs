using discipline.centre.users.domain.Subscriptions.Enums;

namespace discipline.centre.users.domain.Subscriptions.Policies.Abstractions;

public interface ISubscriptionPolicy
{
    bool CanByApplied(SubscriptionType type);
    int? NumberOfDailyTasks();
    int? NumberOfRules();
}