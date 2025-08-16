using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.Stages;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Mappers;

public static class CreateStageRequestDtoMapperExtensions
{
    public static CreateStageForActivityRuleCommand MapAsCommand(this CreateStageRequestDto dto, 
        AccountId accountId, 
        ActivityRuleId activityRuleId,
        StageId stageId)
        => new(accountId, activityRuleId, stageId, dto.Title, dto.Index);
}