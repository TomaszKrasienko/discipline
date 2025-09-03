namespace discipline.daily_trackers.domain.DailyTrackers.Repositories;

public interface IReadWriteUserDailyTrackerRepository : IReadUserDailyTrackerRepository
{
    Task AddAsync(UserDailyTracker dailyTracker, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserDailyTracker dailyTracker, CancellationToken cancellationToken = default);
}