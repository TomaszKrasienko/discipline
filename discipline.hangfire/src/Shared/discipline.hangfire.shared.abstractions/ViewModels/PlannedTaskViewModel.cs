using discipline.hangfire.shared.abstractions.Identifiers;

namespace discipline.hangfire.shared.abstractions.ViewModels;

public sealed record PlannedTaskViewModel(Ulid Id, ActivityRuleId ActivityRuleId, AccountId AccountId);