using discipline.centre.activityrules.domain.Enums;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.activityrules.domain.Rules.ActivityRules;

internal sealed class RuleModeRequireSelectedDaysRule(RuleMode ruleMode,
    List<DayOfWeek>? selectedDays) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRules.RuleModeRequireSelectedDays",
        "Activity rule mode requires selected days");

    public bool IsBroken()
        => ruleMode.IsDaysRequired && selectedDays is null;
}