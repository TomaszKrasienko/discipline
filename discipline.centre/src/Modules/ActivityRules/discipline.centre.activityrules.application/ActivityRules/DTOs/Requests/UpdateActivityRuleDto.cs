using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules;

namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Requests;

public sealed record UpdateActivityRuleDto(ActivityRuleDetailsRequestDto Details,
    ActivityRuleModeRequestDto Mode);