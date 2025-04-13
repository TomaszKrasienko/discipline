using discipline.centre.activityrules.domain.Specifications;

namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.Create;

public sealed record CreateActivityRuleRequestDto(CreateActivityRuleDetailsRequestDto Details, 
    CreateActivityRuleModeRequestDto Mode, List<StageSpecification> Stages);