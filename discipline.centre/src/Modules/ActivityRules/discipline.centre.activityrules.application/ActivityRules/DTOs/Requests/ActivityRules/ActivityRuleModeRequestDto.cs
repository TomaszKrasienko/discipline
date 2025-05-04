namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules;

public sealed record ActivityRuleModeRequestDto(string Mode, List<int>? Days);