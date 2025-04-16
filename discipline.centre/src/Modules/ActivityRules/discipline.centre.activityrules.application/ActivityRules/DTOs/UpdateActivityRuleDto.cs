using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules;

namespace discipline.centre.activityrules.application.ActivityRules.DTOs;

public sealed record UpdateActivityRuleDto(ActivityRuleDetailsRequestDto Details,
    ActivityRuleModeRequestDto Mode);