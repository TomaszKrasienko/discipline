using discipline.daily_trackers.domain.DailyTrackers.Specifications;
using discipline.daily_trackers.domain.DailyTrackers.ValueObjects;
using discipline.daily_trackers.domain.DailyTrackers.ValueObjects.Activities;
using discipline.daily_trackers.domain.SharedKernel;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

namespace discipline.daily_trackers.domain.DailyTrackers;

public sealed class Activity : Entity<ActivityId, Ulid>
{
    private HashSet<Stage> _stages = [];
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

    internal static Activity Create(
        ActivityId activityId,
        ActivityDetailsSpecification details)
        => Create(activityId, null, details);

    internal static Activity CreateFromRule(
        ActivityId activityId,
        ActivityRuleId ruleId,
        ActivityDetailsSpecification details)
        => Create(activityId, ruleId, details);
    
    private static Activity Create(
        ActivityId activityId,
        ActivityRuleId? ruleId,
        ActivityDetailsSpecification details)
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
        string title)
    {
        if(_stages.Any(x => x.Title == title))
        {
            throw new DomainException("ActivityRule.Stages.StageTitleMustBeUnique");
        }
        
        var index = (_stages.Max(x => (int?)x.Index.Value) ?? 0) + 1;
        _stages.Add(Stage.Create(stageId, title, index));
    }
    
    // internal void Edit(ActivityDetailsSpecification details)
    //     => Details = Details.Create(details.Title, details.Note);
    //
    // internal void MarkAsChecked()
    // {
    //     if (_stages.Any())
    //     {
    //         foreach (var stage in _stages)
    //         {
    //             stage.MarkAsChecked();
    //         }
    //     }
    //     
    //     IsChecked = true;
    // } 
    //
    // internal Stage AddStage(string title)
    // {
    //     CheckStageTitleUniqueness(title);
    //     var index = _stages.Max(x => x.Index.Value) + 1;
    //     var stage = Stage.Create(StageId.New(), title, index);
    //     _stages.Add(stage);
    //     return stage;
    // }
    //
    // internal bool DeleteStage(StageId stageId)
    // {
    //     var stage = _stages.SingleOrDefault(x => x.Id == stageId);
    //     
    //     if (stage is null)
    //     {
    //         return false;
    //     }
    //     
    //     _stages.Remove(stage);
    //     
    //     var newStages = _stages!.OrderBy(x => x.Index.Value).ToList();
    //     for (var i = 0; i < newStages.Count; i++)
    //     {
    //         _stages!.First(x => x.Id == newStages[i].Id).ChangeIndex(i + 1);
    //     }
    //
    //     return true;
    // }
    
    // internal void MarkStageAsChecked(StageId stageId)
    // {
    //     var stage = _stages?.SingleOrDefault(x => x.Id == stageId);
    //     
    //     if (stage is null)
    //     {
    //         throw new DomainException("DailyTracker.Activity.StageNotFound",
    //             $"Stage with 'ID': {stageId.ToString()} for activity with 'ID': '{Id.ToString()}' was not found.");
    //     }
    //     
    //     stage.MarkAsChecked();
    //
    //     if (_stages is not null && _stages.Any(x => !x.IsChecked.Value))
    //     {
    //         return;
    //     }
    //     
    //     MarkAsChecked();
    // }
    //
    // public void ClearParentActivityRuleId()
    //     => ParentActivityRuleId = null;
}