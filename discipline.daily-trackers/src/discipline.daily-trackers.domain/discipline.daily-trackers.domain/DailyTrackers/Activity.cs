using discipline.daily_trackers.domain.DailyTrackers.Specifications;
using discipline.daily_trackers.domain.DailyTrackers.ValueObjects;
using discipline.daily_trackers.domain.DailyTrackers.ValueObjects.Activities;
using discipline.daily_trackers.domain.SharedKernel;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

namespace discipline.daily_trackers.domain.DailyTrackers;

public sealed class Activity : Entity<ActivityId, Ulid>
{
    private readonly HashSet<Stage> _stages = [];
    public Details Details { get; private set; }
    public IsChecked IsChecked { get; private set; }
    public ActivityRuleId? ParentActivityRuleId { get; private set; }
    public IReadOnlyCollection<Stage> Stages => _stages.ToArray();
    
    /// <summary>
    /// <remarks>Only for EF purposes</remarks>
    /// </summary>
    public Activity(
        ActivityId id,
        Details details,
        IsChecked isChecked, 
        ActivityRuleId? parentActivityRuleId,
        HashSet<Stage> stages) : this(
            id,
            details,
            isChecked,
            parentActivityRuleId)  
    {
        _stages = stages;
    }

    private Activity(
        ActivityId id,
        Details details,
        IsChecked isChecked,
        ActivityRuleId? parentActivityRuleId) : base(id)  
    {
        Details = details;
        IsChecked = isChecked;
        ParentActivityRuleId = parentActivityRuleId;
    }

    internal static Activity CreateFromRule(
        ActivityId activityId,
        ActivityRuleId ruleId,
        ActivityDetailsSpecification details,
        IReadOnlyCollection<StageSpecification> stages)
        => Create(activityId, ruleId, details, stages);
    
    private static Activity Create(
        ActivityId activityId,
        ActivityRuleId? ruleId,
        ActivityDetailsSpecification details,
        IReadOnlyCollection<StageSpecification> stages)
    {
        var activityDetails = Details.Create(details.Title, details.Note);
        
        var activity = new Activity(
            activityId,
            activityDetails,
            false,
            ruleId);

        return activity;
    } 

    internal void AddStage(
        StageId stageId,
        string title,
        int index)
    {
        if (IsStageTitleExists(title))
        {
            throw new DomainException("UserDailyTracker.Activity.Stage.TitleAlreadyExists");
        }
        
        if (IsStageIndexExists(index))
        {
            throw new DomainException("UserDailyTracker.Activity.Stage.IndexAlreadyExists");
        }
        
        _stages.Add(Stage.Create(stageId, title, index));
    }

    private bool IsStageTitleExists(string title)
        => Stages.Any(x => x.Title == title);
    
    private bool IsStageIndexExists(int index)
        => Stages.Any(x => x.Index == index);
}