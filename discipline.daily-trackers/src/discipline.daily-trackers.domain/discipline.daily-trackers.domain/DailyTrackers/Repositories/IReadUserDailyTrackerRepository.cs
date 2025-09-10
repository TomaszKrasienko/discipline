using discipline.daily_trackers.domain.DailyTrackers.ValueObjects.DailyTrackers;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

namespace discipline.daily_trackers.domain.DailyTrackers.Repositories;

public interface IReadUserDailyTrackerRepository
{
    Task<UserDailyTracker?> GetByDayAsync(
        AccountId  accountId,
        Day day,
        CancellationToken cancellationToken = default);
    
    Task<bool> DoesExistAsync(
        AccountId accountId,
        Day day,
        CancellationToken cancellationToken = default);
}