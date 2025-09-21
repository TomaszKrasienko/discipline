using discipline.daily_trackers.domain.DailyTrackers.Repositories;
using discipline.daily_trackers.domain.DailyTrackers.Specifications;
using discipline.daily_trackers.domain.DailyTrackers.ValueObjects.DailyTrackers;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

namespace discipline.daily_trackers.domain.DailyTrackers.Services;

public interface IUserDailyTrackerFactory
{
    // Task<UserDailyTracker> Create(
    //     DailyTrackerId id,
    //     AccountId accountId,
    //     Day day,
    //     ActivityId activityId,
    //     ActivityDetailsSpecification details,
    //     ActivityRuleId? parentActivityRuleId,
    //     CancellationToken cancellationToken = default);
    
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
    // public async Task<UserDailyTracker> Create(
    //     DailyTrackerId id,
    //     AccountId accountId,
    //     Day day,
    //     ActivityId activityId,
    //     ActivityDetailsSpecification details,
    //     ActivityRuleId? parentActivityRuleId,
    //     CancellationToken cancellationToken = default)
    // {
    //     var priorUserDailyTracker = await userDailyTrackerRepository.GetByDayAsync(
    //         accountId,
    //         day.GetPriorDay(),
    //         cancellationToken);
    //
    //     DailyTrackerId? prior = null;
    //     
    //     if (priorUserDailyTracker is not null)
    //     {
    //         prior = priorUserDailyTracker.Id;
    //         priorUserDailyTracker.SetNext(id);
    //     }
    //     
    //     var userDailyTracker = UserDailyTracker.Create(
    //         id,
    //         accountId,
    //         day,
    //         activityId,
    //         details,
    //         parentActivityRuleId,
    //         prior);
    //     
    //     await userDailyTrackerRepository.AddAsync(userDailyTracker, cancellationToken);
    //
    //     if (priorUserDailyTracker is not null)
    //     {
    //         await userDailyTrackerRepository.UpdateAsync(priorUserDailyTracker, cancellationToken);
    //     }
    //     
    //     return userDailyTracker;
    // }

    public async Task<UserDailyTracker> Create(
        DailyTrackerId id,
        AccountId accountId,
        Day day,
        CancellationToken cancellationToken = default)
    {
        var priorUserDailyTracker = await userDailyTrackerRepository.GetByDayAsync(
            accountId,
            day.GetPriorDay(),
            cancellationToken);

        DailyTrackerId? prior = null;
        
        if (priorUserDailyTracker is not null)
        {
            prior = priorUserDailyTracker.Id;
            priorUserDailyTracker.SetNext(id);
        }
        
        var userDailyTracker = UserDailyTracker.Create(
            id,
            accountId,
            day,
            prior);
        
        await userDailyTrackerRepository.AddAsync(userDailyTracker, cancellationToken);

        if (priorUserDailyTracker is not null)
        {
            await userDailyTrackerRepository.UpdateAsync(priorUserDailyTracker, cancellationToken);
        }
        
        return userDailyTracker;
    }

    public async Task<UserDailyTracker> Create(
        DailyTrackerId id,
        AccountId accountId,
        Day day,
        IReadOnlyCollection<ActivitySpecification> activities,
        CancellationToken cancellationToken = default)
    {
        var priorUserDailyTracker = await userDailyTrackerRepository.GetByDayAsync(
            accountId,
            day.GetPriorDay(),
            cancellationToken);
        
        DailyTrackerId? prior = null;
        
        if (priorUserDailyTracker is not null)
        {
            prior = priorUserDailyTracker.Id;
            priorUserDailyTracker.SetNext(id);
        }
        
        var userDailyTracker = UserDailyTracker.Create(
            id,
            accountId,
            day,
            prior,
            activities);
        
        await userDailyTrackerRepository.AddAsync(userDailyTracker, cancellationToken);

        if (priorUserDailyTracker is not null)
        {
            await userDailyTrackerRepository.UpdateAsync(priorUserDailyTracker, cancellationToken);
        }
        
        return userDailyTracker;
    }
}