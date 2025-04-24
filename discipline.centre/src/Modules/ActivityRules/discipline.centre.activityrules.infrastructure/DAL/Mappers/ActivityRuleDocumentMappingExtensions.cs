using discipline.centre.activityrules.application.ActivityRules.DTOs.Responses;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Enums;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

// ReSharper disable once CheckNamespace
namespace discipline.centre.activityrules.infrastructure.DAL.Documents;

internal static class ActivityRuleDocumentMappingExtensions
{
    internal static ActivityRule AsEntity(this ActivityRuleDocument document)
        => new(ActivityRuleId.Parse(document.Id),
            UserId.Parse(document.UserId),
            document.Details.AsEntity(),
            document.SelectedMode.AsEntity(),
            document.Stages.Select(x => x.AsEntity()).ToList());

    private static Details AsEntity(this ActivityRuleDetailsDocument document)
        => Details.Create(document.Title, document.Note);
    
    private static SelectedMode AsEntity(this ActivityRuleSelectedModeDocument document)
        => SelectedMode.Create(RuleMode.Parse(document.Mode), document.DaysOfWeek?.ToHashSet());
    
    private static Stage AsEntity(this StageDocument document)
        => new (StageId.Parse(document.StageId), document.Title, document.Index);

    internal static ActivityRuleResponseDto AsResponseDto(this ActivityRuleDocument document)
        => new(document.Id, 
            document.Details.AsResponseDto(), 
            document.SelectedMode.AsResponseDto(), 
            document.Stages.Select(x => x.AsResponseDto()).ToList());

    private static DetailsResponseDto AsResponseDto(this ActivityRuleDetailsDocument document)
        => new(document.Title, document.Note);
    
    private static SelectedModeResponseDto AsResponseDto(this ActivityRuleSelectedModeDocument document)
        => new(document.Mode, document.DaysOfWeek?.ToHashSet());
    
    private static StageResponseDto AsResponseDto(this StageDocument document)
        => new(document.StageId, document.Title, document.Index);
}