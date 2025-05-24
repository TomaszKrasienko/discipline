using discipline.centre.activityrules.domain.Events;
using discipline.centre.activityrules.domain.Rules;
using discipline.centre.activityrules.domain.Rules.ActivityRules;
using discipline.centre.activityrules.domain.Rules.Stages;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.activityrules.domain.ValueObjects.Stages;
using discipline.centre.shared.abstractions.SharedKernel.Aggregate;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain;

public sealed class ActivityRule : AggregateRoot<ActivityRuleId, Ulid>
{
    private readonly List<Stage> _stages = [];
    public UserId UserId { get; }
    public Details Details { get; private set; }
    public SelectedMode Mode { get; private set; }
    public IReadOnlyList<Stage> Stages => _stages.ToArray();
    
    /// <summary>
    /// Constructor for mapping to mongo documents
    /// </summary>
    public ActivityRule(ActivityRuleId id, UserId userId, Details details,
        SelectedMode mode, List<Stage> stages) : base(id)
    {   
        UserId = userId;
        Details = details;  
        Mode = mode;
        _stages = stages;   
    }
    
    private ActivityRule(ActivityRuleId id, UserId userId, Details details,
        SelectedMode mode) : base(id)
    {        
        UserId = userId;
        Details = details;  
        Mode = mode;
        
        AddDomainEvent(new ActivityRuleCreated(id, userId, details, mode));
    }
    
    public static ActivityRule Create(ActivityRuleId id, 
        UserId userId, 
        ActivityRuleDetailsSpecification details, 
        ActivityRuleModeSpecification mode)
    {
        var activityRuleDetails = Details.Create(details.Title, details.Note);
        var activityRuleMode = SelectedMode.Create(mode.Mode, mode.Days);
        var activityRule = new ActivityRule(id, userId, activityRuleDetails, activityRuleMode);
        
        return activityRule;
    }

    public void Edit(
        ActivityRuleDetailsSpecification details, 
        ActivityRuleModeSpecification mode)
    {
        Details = Details.Create(details.Title, details.Note);
        Mode = SelectedMode.Create(mode.Mode, mode.Days);
        
        AddDomainEvent(new ActivityRuleChanged(Id, UserId, Details, Mode));
    }

    public Stage AddStage(
        StageId stageId, 
        string title)
    {
        CheckRule(new StageTitleMustBeUniqueRule(_stages, title));
        var index = Stages.Count == 0 
            ? 0 
            : Stages
                .Select(x => x.Index.Value)
                .Max(x => x);

        var stage = Stage.Create(stageId, title, index + 1);
        _stages.Add(stage);
        
        return stage;
    }

    public bool RemoveStage(StageId stageId)
    {
        var stage = Stages.SingleOrDefault(x => x.Id == stageId);

        if (stage is null)
        {
            return false;
        }
        
        _stages.Remove(stage);

        foreach (var s in _stages
                     .OrderBy(x => x.Index.Value)
                     .Select((value, index) => new { value, index }))
        {
            s.value.UpdateIndex(s.index + 1);
        }
        
        return true;
    }
}