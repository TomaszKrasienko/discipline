using System.Collections.Immutable;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.activityrules.infrastructure.DAL.Documents;

// ReSharper disable once CheckNamespace
namespace discipline.centre.activityrules.domain;

internal static class ActivityRuleMappingExtensions
{
    internal static ActivityRuleDocument AsDocument(this ActivityRule entity)
        => new()
        {
            Id = entity.Id.Value.ToString(),
            UserId = entity.AccountId.ToString(),
            Details = entity.Details.AsDocument(),
            SelectedMode = entity.Mode.AsDocument(),
            Stages = entity.Stages.Select(x => x.AsDocument()),
        };

    private static ActivityRuleDetailsDocument AsDocument(this Details entity) 
        => new(entity.Title, entity.Note);

    private static ActivityRuleSelectedModeDocument AsDocument(this SelectedMode entity)
        => new(entity.Mode.Value, entity.Days?.Select(x => (int)x).ToImmutableHashSet());

    private static StageDocument AsDocument(this Stage stage)
        => new(stage.Id.ToString(), stage.Title, stage.Index);
}