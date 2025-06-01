namespace discipline.ui.infrastructure.DTOs.ActivityRules;

public sealed record ActivityRuleSelectedModeDto(string Mode, HashSet<int>? Days);