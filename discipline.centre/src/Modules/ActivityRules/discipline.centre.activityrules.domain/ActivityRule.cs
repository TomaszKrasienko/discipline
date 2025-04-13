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
        SelectedMode mode, List<Stage> stages) : this(id, userId, details, mode)
    {        
        _stages = stages;   
    }
    
    private ActivityRule(ActivityRuleId id, UserId userId, Details details,
        SelectedMode mode) : base(id)
    {        
        UserId = userId;
        Details = details;  
        Mode = mode;
        
        AddDomainEvent(new ActivityRuleCreated(id, userId));
    }
    
    public static ActivityRule Create(ActivityRuleId id, 
        UserId userId, 
        ActivityRuleDetailsSpecification details, 
        ActivityRuleModeSpecification mode, 
        List<StageSpecification> stages)
    {
        var activityRuleDetails = Details.Create(details.Title, details.Note);
        var activityRuleMode = SelectedMode.Create(mode.Mode, mode.Days);
        var activityRule = new ActivityRule(id, userId, activityRuleDetails, activityRuleMode);
        activityRule.AddStages(stages);
        
        return activityRule;
    }

    public void Edit(ActivityRuleDetailsSpecification details, 
        ActivityRuleModeSpecification mode)
    {
        if (!HasChanges(details, mode))
        {
            throw new DomainException("ActivityRule.NoChanges",
                "Activity rule has no changes");
        }
        Details = Details.Create(details.Title, details.Note);
        Mode = SelectedMode.Create(mode.Mode, mode.Days);;
    }
    
    public bool HasChanges(ActivityRuleDetailsSpecification details,
        ActivityRuleModeSpecification mode)
        => (Details.HasChanges(details.Title, details.Note));

    private void AddStages(List<StageSpecification> stages)
        => stages.ForEach(x => AddStage(x));

    public Stage AddStage(StageSpecification stage)
    {
        CheckRule(new StagesMustHaveOrderedIndexRule(_stages, stage));
        CheckRule(new StageTitleMustBeUniqueRule(_stages, stage));
        var newStage = Stage.Create(StageId.New(), stage.Title, stage.Index);
        _stages.Add(newStage);
        return newStage;
    }
}