using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.activityrules.domain.Rules.ActivityRules;

internal sealed class SelectedDayCanNotBeOutOfRangeRule(int value) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRule.Mode.SelectedDayOutOfRange");

    public bool IsBroken()
        => value is < (int)DayOfWeek.Sunday or > (int)DayOfWeek.Saturday;
}