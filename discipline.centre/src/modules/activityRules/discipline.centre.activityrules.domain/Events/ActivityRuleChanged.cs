using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel;

using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain.Events;

public sealed record ActivityRuleChanged(
    ActivityRuleId ActivityRuleId,
    AccountId AccountId, 
    Details Details,
    SelectedMode Mode) : DomainEvent;