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
            UserId = entity.UserId.ToString(),
            Details = entity.Details.AsDocument(),
            SelectedMode = entity.Mode.AsDocument(),
            Stages = entity.Stages.Select(x => x.MapAsDocument()),
        };

    private static ActivityRuleDetailsDocument AsDocument(this Details entity) 
        => new(entity.Title, entity.Note);

    private static ActivityRuleSelectedModeDocument AsDocument(this SelectedMode entity)
        => new(entity.Mode.Value, entity.Days?.Select(x => (int)x).ToImmutableHashSet());
}