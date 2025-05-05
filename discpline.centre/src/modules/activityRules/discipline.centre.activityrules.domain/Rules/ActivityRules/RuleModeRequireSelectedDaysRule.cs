using discipline.centre.activityrules.domain.Enums;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.activityrules.domain.Rules.ActivityRules;

internal sealed class RuleModeRequireSelectedDaysRule(RuleMode ruleMode,
    HashSet<int>? selectedDays) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRules.Mode.RuleModeRequireSelectedDays");

    public bool IsBroken()
        => ruleMode.IsDaysRequired && selectedDays is null;
}