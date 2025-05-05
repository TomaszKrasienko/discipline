namespace discipline.ui.communication.http.ActivityRules.DTOs;

public sealed record ActivityRuleModeRequestDto(string Mode, HashSet<int>? Days);