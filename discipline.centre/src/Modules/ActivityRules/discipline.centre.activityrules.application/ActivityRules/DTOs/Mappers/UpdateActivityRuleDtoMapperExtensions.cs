using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests;
using discipline.centre.activityrules.domain.Enums;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

// ReSharper disable once CheckNamespace
namespace discipline.centre.activityrules.application.ActivityRules.DTOs;

public static class UpdateActivityRuleDtoMapperExtensions
{
    public static UpdateActivityRuleCommand AsCommand(this UpdateActivityRuleDto dto, UserId userId,
        ActivityRuleId activityRuleId)
    {
        var detailsSpecification = new ActivityRuleDetailsSpecification(dto.Details.Title, dto.Details.Note);
        
        var mode = RuleMode.Parse(dto.Mode.Mode);
        var modeSpecification = new ActivityRuleModeSpecification(mode, dto.Mode.Days?.ToHashSet());
        
        return new UpdateActivityRuleCommand(userId, activityRuleId, detailsSpecification, modeSpecification);
    }
}