using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.domain.Repositories;

public interface IWriteDailyTrackerRepository : IReadDailyTrackerRepository
{
    Task AddAsync(DailyTracker dailyTracker, CancellationToken cancellationToken);
    Task UpdateAsync(DailyTracker dailyTracker, CancellationToken cancellationToken);
    Task UpdateRangeAsync(IEnumerable<DailyTracker> dailyTrackers, CancellationToken cancellationToken);
    Task DeleteAsync(DailyTracker dailyTracker, CancellationToken cancellationToken);
}