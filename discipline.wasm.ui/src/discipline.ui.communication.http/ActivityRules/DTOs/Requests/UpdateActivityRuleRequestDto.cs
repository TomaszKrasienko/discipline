namespace discipline.ui.communication.http.ActivityRules.DTOs.Requests;

public sealed record UpdateActivityRuleRequestDto(ActivityRuleDetailsRequestDto Details, 
    ActivityRuleModeRequestDto Mode);