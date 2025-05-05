namespace discipline.ui.communication.http.ActivityRules.DTOs.Requests;

public sealed record CreateActivityRuleRequestDto(ActivityRuleDetailsRequestDto Details, 
    ActivityRuleModeRequestDto Mode);
