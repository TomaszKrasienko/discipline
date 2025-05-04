namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules;

public sealed record ActivityRuleRequestDto(ActivityRuleDetailsRequestDto Details, 
    ActivityRuleModeRequestDto Mode);