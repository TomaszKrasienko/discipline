using System.Collections.Immutable;
using discipline.centre.activity_rules.infrastructure.DAL.Documents;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.activityrules.infrastructure.DAL.Documents;

// ReSharper disable once CheckNamespace
namespace discipline.centre.activityrules.domain;

internal static class ActivityRuleMappingExtensions
{
    internal static ActivityRuleDocument ToDocument(this ActivityRule entity)
        => new()
        {
            Id = entity.Id.Value.ToString(),
            AccountId = entity.AccountId.ToString(),
            Details = entity.Details.ToDocument(),
            SelectedMode = entity.Mode.ToDocument(),
            Stages = entity.Stages.Select(x => x.ToDocument()),
        };

    private static ActivityRuleDetailsDocument ToDocument(this Details entity) 
        => new(entity.Title, entity.Note);

    private static ActivityRuleSelectedModeDocument ToDocument(this SelectedMode entity)
        => new(entity.Mode.Value, entity.Days?.Select(x => (int)x).ToImmutableHashSet());

    private static StageDocument ToDocument(this Stage stage)
        => new(stage.Id.ToString(), stage.Title, stage.Index);
}