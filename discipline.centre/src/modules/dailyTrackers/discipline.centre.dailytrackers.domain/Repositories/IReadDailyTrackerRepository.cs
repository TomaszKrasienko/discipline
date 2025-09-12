using discipline.centre.dailytrackers.domain.ValueObjects.DailyTrackers;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.domain.Repositories;

public interface IReadDailyTrackerRepository
{
    Task<bool> ExistsAsync(AccountId accountId, Day day, CancellationToken cancellationToken);
    Task<DailyTracker?> GetDailyTrackerByDayAsync(AccountId accountId, DateOnly day, CancellationToken cancellationToken);
    Task<DailyTracker?> GetDailyTrackerByIdAsync(AccountId accountId, DailyTrackerId id, CancellationToken cancellationToken);
    Task<List<DailyTracker>> GetDailyTrackersByParentActivityRuleId(AccountId accountId, ActivityRuleId activityRuleId, CancellationToken cancellationToken);
}