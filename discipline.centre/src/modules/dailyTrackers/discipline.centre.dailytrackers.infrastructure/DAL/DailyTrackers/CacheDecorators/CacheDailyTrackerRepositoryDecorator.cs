using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.centre.shared.abstractions.Cache;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.CacheDecorators;

internal sealed class CacheDailyTrackerRepositoryDecorator(
    IReadWriteDailyTrackerRepository readWriteDailyTrackerRepository,
    ICacheFacade cacheFacade) : IReadWriteDailyTrackerRepository
{
    public async Task<DailyTracker?> GetDailyTrackerByDayAsync(AccountId accountId, DateOnly day, CancellationToken cancellationToken)
    {
        var cachedData = await cacheFacade.GetAsync<DailyTrackerDocument>(GetCacheKey(accountId, day.ToString()), cancellationToken);

        if (cachedData is not null)
        {
            return cachedData.AsEntity();
        }
        
        var result = await readWriteDailyTrackerRepository.GetDailyTrackerByDayAsync(accountId, day, cancellationToken);

        if (result is null)
        {
            return null;
        }

        await AddOrUpdateToCacheAsync(result, cancellationToken);
        return result;

    }

    public async Task<DailyTracker?> GetDailyTrackerByIdAsync(AccountId accountId, DailyTrackerId id, CancellationToken cancellationToken)
    {
        var cachedData = await cacheFacade.GetAsync<DailyTrackerDocument>(GetCacheKey(accountId, id.ToString()), cancellationToken);

        if (cachedData is not null)
        {
            return cachedData.AsEntity();
        }
        
        var result = await readWriteDailyTrackerRepository.GetDailyTrackerByIdAsync(accountId, id, cancellationToken);

        if (result is null)
        {
            return null;
        }
            
        await AddOrUpdateToCacheAsync(result, cancellationToken);
            
        return result;
    }

    public Task<List<DailyTracker>> GetDailyTrackersByParentActivityRuleId(AccountId accountId, ActivityRuleId activityRuleId,
        CancellationToken cancellationToken)
        => readWriteDailyTrackerRepository.GetDailyTrackersByParentActivityRuleId(accountId, activityRuleId,
            cancellationToken);

    public async Task AddAsync(DailyTracker dailyTracker, CancellationToken cancellationToken)
    {
        await AddOrUpdateToCacheAsync(dailyTracker, cancellationToken);
        await readWriteDailyTrackerRepository.AddAsync(dailyTracker, cancellationToken);
    }

    public async Task UpdateAsync(DailyTracker dailyTracker, CancellationToken cancellationToken)
    {
        await AddOrUpdateToCacheAsync(dailyTracker, cancellationToken);
        await readWriteDailyTrackerRepository.UpdateAsync(dailyTracker, cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<DailyTracker> dailyTrackers, CancellationToken cancellationToken)
    {
        var trackers = dailyTrackers.ToList();
        
        foreach (var dailyTracker in trackers)
        {
            await RemoveFromCacheAsync(dailyTracker, cancellationToken);
        }
        
        await readWriteDailyTrackerRepository.UpdateRangeAsync(trackers, cancellationToken);
    }

    public async Task DeleteAsync(DailyTracker dailyTracker, CancellationToken cancellationToken)
    {
        await RemoveFromCacheAsync(dailyTracker, cancellationToken);
        await readWriteDailyTrackerRepository.DeleteAsync(dailyTracker, cancellationToken);
    }

    private async Task AddOrUpdateToCacheAsync(DailyTracker dailyTracker, CancellationToken cancellationToken)
    {
        await cacheFacade.AddOrUpdateAsync(GetCacheKey(dailyTracker.AccountId, dailyTracker.Day.Value.ToString()), 
            dailyTracker.AsDocument(), cancellationToken);
        await cacheFacade.AddOrUpdateAsync(GetCacheKey(dailyTracker.AccountId, dailyTracker.Id.ToString()), 
            dailyTracker.AsDocument(), cancellationToken);
    }

    private async Task RemoveFromCacheAsync(DailyTracker dailyTracker, CancellationToken cancellationToken)
    {
        await cacheFacade.DeleteAsync(GetCacheKey(dailyTracker.AccountId, dailyTracker.Day.Value.ToString()),
            cancellationToken);
        await cacheFacade.DeleteAsync(GetCacheKey(dailyTracker.AccountId,dailyTracker.Id.ToString()),
            cancellationToken);
    }
    
    internal static string GetCacheKey(AccountId accountId, string param)
        => $"{accountId.ToString()}:{param}";
}