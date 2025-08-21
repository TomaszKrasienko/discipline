using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using MongoDB.Driver;

namespace discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Repositories;

internal sealed class MongoDailyTrackerRepository(
    DailyTrackersMongoContext context) : IReadWriteDailyTrackerRepository
{
    public async Task<DailyTracker?> GetDailyTrackerByDayAsync(AccountId accountId, 
        DateOnly day,
        CancellationToken cancellationToken = default)
        => (await context.GetCollection<DailyTrackerDocument>().Find(x
                => x.Day == day
                && x.AccountId == accountId.ToString())
            .SingleOrDefaultAsync(cancellationToken))?.AsEntity();

    public async Task<DailyTracker?> GetDailyTrackerByIdAsync(AccountId accountId, 
        DailyTrackerId id,
        CancellationToken cancellationToken = default)
        => (await context.GetCollection<DailyTrackerDocument>().Find(x
                => x.DailyTrackerId == id.ToString() 
                && x.AccountId == accountId.ToString())
            .SingleOrDefaultAsync(cancellationToken))?.AsEntity();

    public async Task<List<DailyTracker>> GetDailyTrackersByParentActivityRuleId(AccountId accountId, 
        ActivityRuleId activityRuleId,
        CancellationToken cancellationToken = default)
        => (await context.GetCollection<DailyTrackerDocument>()
                .Find(dt => dt.Activities.Any(activity
                                => activity.ParentActivityRuleId == activityRuleId.ToString())
                            && dt.AccountId == accountId.ToString())
                .ToListAsync(cancellationToken))
            .Select(x => x.AsEntity()).ToList();
        

    public Task AddAsync(DailyTracker dailyTracker, CancellationToken cancellationToken = default)
        => context.GetCollection<DailyTrackerDocument>()
            .InsertOneAsync(dailyTracker.AsDocument(), cancellationToken: cancellationToken);

    public Task UpdateAsync(DailyTracker dailyTracker, CancellationToken cancellationToken = default)
        => context.GetCollection<DailyTrackerDocument>()
            .FindOneAndReplaceAsync(x => x.DailyTrackerId == dailyTracker.Id.ToString(),
                dailyTracker.AsDocument(), null, cancellationToken);

    public async Task UpdateRangeAsync(IEnumerable<DailyTracker> dailyTrackers, CancellationToken cancellationToken = default)
    {
        var tasks = new List<Task>();
        
        foreach (var dailyTracker in dailyTrackers)
        {
            tasks.Add(UpdateAsync(dailyTracker, cancellationToken));
        }
        
        await Task.WhenAll(tasks);
    }

    public Task DeleteAsync(DailyTracker dailyTracker, CancellationToken cancellationToken)
        => context.GetCollection<DailyTrackerDocument>().DeleteOneAsync(x => x.DailyTrackerId == dailyTracker.Id.ToString(),
            cancellationToken);
}