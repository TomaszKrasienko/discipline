using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.dailytrackers.domain.ValueObjects.DailyTrackers;
using discipline.centre.shared.abstractions.SharedKernel.Aggregate;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.domain;

public sealed class DailyTracker : AggregateRoot<DailyTrackerId, Ulid>
{
    private readonly List<Activity> _activities = [];
    public Day Day { get; private set; }
    public AccountId AccountId { get; private set; }

    public IReadOnlyCollection<Activity> Activities => _activities;

    /// <summary>
    /// <remarks>Use only for Mongo purposes</remarks>
    /// </summary>
    public DailyTracker(DailyTrackerId id, Day day, AccountId accountId, List<Activity> activities) : this(id,
        day, accountId)
        => _activities = activities;

    private DailyTracker(DailyTrackerId id, Day day, AccountId accountId) : base(id)
    {
        Day = day;
        AccountId = accountId;
    }

    public static DailyTracker Create(DailyTrackerId id, DateOnly day, AccountId accountId, ActivityId activityId,
        ActivityDetailsSpecification details, ActivityRuleId? parentActivityRuleId, List<StageSpecification>? stages)
    {
        var dailyTracker = new DailyTracker(id, day, accountId);
        dailyTracker.AddActivity(activityId, details, parentActivityRuleId, stages);
        return dailyTracker;
    }

    public Activity AddActivity(ActivityId activityId, ActivityDetailsSpecification details,
        ActivityRuleId? parentActivityRuleId, List<StageSpecification>? stages)
    {
        if (_activities.Exists(x => x.Details.Title == details.Title))
        {
            throw new DomainException("DailyTracker.Activity.Title.AlreadyExists",
                $"Activity with title '{details.Title}' already exists.");
        }
        
        var activity = Activity.Create(activityId, details, parentActivityRuleId, stages);
        _activities.Add(activity);
        return activity;
    }

    public void MarkActivityAsChecked(ActivityId activityId)
    {
        var activity = _activities.SingleOrDefault(x => x.Id == activityId);

        if (activity is null)
        {
            throw new DomainException("DailyTracker.Activity.NotExists",
                $"Activity with ID: '{activityId}' does not exist.");
        }
        
        activity.MarkAsChecked();
    }

    public bool DeleteActivity(ActivityId activityId)
    {
        var activity = _activities.SingleOrDefault(x => x.Id == activityId);

        if (activity is null)
        {
            return false;
        }
        
        _activities.Remove(activity);
        return true;
    }
    
    public void MarkActivityStageAsChecked(ActivityId activityId, StageId stageId)
    {
        var activity = _activities.SingleOrDefault(x => x.Id == activityId);
        
        if (activity is null)
        {
            throw new DomainException("DailyTracker.Activity.NotFound",
                $"Activity with id {activityId} not found.");    
        }
        
        activity.MarkStageAsChecked(stageId);
    }

    public bool DeleteActivityStage(ActivityId activityId, StageId stageId)
    {
        var activity = _activities.SingleOrDefault(x => x.Id == activityId);

        if (activity is null)
        {
            return false;
        }
        
        return activity.DeleteStage(stageId);
    }

    public void ClearParentActivityRuleIdIs(ActivityRuleId parentActivityRuleId)
    {
        foreach (var activity in _activities.Where(x => x.ParentActivityRuleId == parentActivityRuleId))
        {
            activity.ClearParentActivityRuleId();
        }
    }
}