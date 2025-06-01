namespace discipline.ui.infrastructure.Clients.DTOs.ActivityRules.Requests;

internal sealed record CreateActivityRuleRequestDto(ActivityRuleDetailsRequestDto Details, 
    ActivityRuleModeRequestDto Mode);
