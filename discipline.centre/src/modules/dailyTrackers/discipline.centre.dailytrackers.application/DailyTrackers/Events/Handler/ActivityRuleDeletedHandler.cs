using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Microsoft.Extensions.Logging;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Events.Handler;

internal sealed class ActivityRuleDeletedHandler(IWriteDailyTrackerRepository readWriteDailyTrackerRepository) : IEventHandler<ActivityRuleDeleted>
{
    //TODO: string type or implicit operator in strogly type ids?
    public async Task HandleAsync(ActivityRuleDeleted @event, CancellationToken cancellationToken)
    {
        var activityRuleId = new ActivityRuleId(@event.ActivityRuleId);
        var accountId = new AccountId(@event.AccountId);

        var dailyTrackers = await readWriteDailyTrackerRepository
            .GetDailyTrackersByParentActivityRuleId(accountId, activityRuleId, cancellationToken);

        foreach (var dailyTracker in dailyTrackers)
        {
            dailyTracker.ClearParentActivityRuleIdIs(activityRuleId);
        }

        await readWriteDailyTrackerRepository.UpdateRangeAsync(dailyTrackers, cancellationToken);
    }
}