using discipline.daily_trackers.domain.DailyTrackers.Repositories;
using discipline.daily_trackers.domain.DailyTrackers.Specifications;
using discipline.daily_trackers.domain.DailyTrackers.ValueObjects.DailyTrackers;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

namespace discipline.daily_trackers.domain.DailyTrackers.Services;

//TODO: Unit tests
public interface IUserDailyTrackerFactory
{
    Task<UserDailyTracker> Create(
        AccountId accountId,
        DateOnly day,
        ActivityId activityId,
        ActivityDetailsSpecification activityDetails,
        CancellationToken cancellationToken = default);
    
    Task<UserDailyTracker> Create(
        DailyTrackerId id,
        AccountId accountId,
        Day day,
        CancellationToken cancellationToken = default);

    Task<UserDailyTracker> Create(
        DailyTrackerId id,
        AccountId accountId,
        Day day,
        IReadOnlyCollection<ActivitySpecification> activities,
        CancellationToken cancellationToken = default);
}

internal sealed class UserDailyTrackerFactory(
    IReadWriteUserDailyTrackerRepository userDailyTrackerRepository) : IUserDailyTrackerFactory
{
    public async Task<UserDailyTracker> Create(
        AccountId accountId,
        DateOnly day,
        ActivityId activityId,
        ActivityDetailsSpecification activityDetails,
        CancellationToken cancellationToken = default)
    {
        var dailyTracker = await userDailyTrackerRepository
            .GetByDayAsync(
                accountId, 
                day,
                cancellationToken);

        if (dailyTracker is null)
        {
            var currentDailyTrackerId = DailyTrackerId.New();
        
            var priorDailyTrackerId = await ResolvePriorAndSetNextAsync(
                accountId,
                currentDailyTrackerId,
                day,
                cancellationToken);    
            
            var userDailyTracker = UserDailyTracker.Create(
                currentDailyTrackerId,
                accountId,
                day,
                priorDailyTrackerId,
                activityId,
                activityDetails);
            
            await userDailyTrackerRepository.AddAsync(userDailyTracker);
            
            return userDailyTracker;
        }
        
        dailyTracker.AddActivity(
            activityId,
            activityDetails);
        
        return dailyTracker;
    }

    public async Task<UserDailyTracker> Create(
        DailyTrackerId id,
        AccountId accountId,
        Day day,
        CancellationToken cancellationToken = default)
    {
        var priorId = await ResolvePriorAndSetNextAsync(
            accountId,
            id,
            day,
            cancellationToken);

        var userDailyTracker = UserDailyTracker.Create(
            id,
            accountId,
            day,
            priorId);

        await userDailyTrackerRepository.AddAsync(userDailyTracker, cancellationToken);

        return userDailyTracker;
    }

    public async Task<UserDailyTracker> Create(
        DailyTrackerId id,
        AccountId accountId,
        Day day,
        IReadOnlyCollection<ActivitySpecification> activities,
        CancellationToken cancellationToken = default)
    {
        var priorId = await ResolvePriorAndSetNextAsync(
            accountId,
            id,
            day,
            cancellationToken);

        var userDailyTracker = UserDailyTracker.Create(
            id,
            accountId,
            day,
            priorId,
            activities);

        await userDailyTrackerRepository.AddAsync(userDailyTracker, cancellationToken);

        return userDailyTracker;
    }
    
    private async Task<DailyTrackerId?> ResolvePriorAndSetNextAsync(
        AccountId accountId,
        DailyTrackerId currentDailyTrackerId,
        Day currentDay,
        CancellationToken cancellationToken = default)
    {
        var priorDay = currentDay.GetPriorDay();
        
        var priorUserDailyTracker = await userDailyTrackerRepository.GetByDayAsync(
            accountId,
            priorDay,
            cancellationToken);

        DailyTrackerId? priorIdentifier = null;

        if (priorUserDailyTracker is not null)
        {
            priorIdentifier = priorUserDailyTracker.Id;
            priorUserDailyTracker.SetNext(currentDailyTrackerId);
            await userDailyTrackerRepository.UpdateAsync(priorUserDailyTracker, cancellationToken);
        }

        return priorIdentifier;
    }
}