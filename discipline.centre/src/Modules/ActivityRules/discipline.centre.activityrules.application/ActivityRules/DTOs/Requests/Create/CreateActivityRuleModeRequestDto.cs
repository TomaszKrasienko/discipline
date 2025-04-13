namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.Create;

public sealed record CreateActivityRuleModeRequestDto(string Mode, List<int>? Days);