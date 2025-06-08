using discipline.daily_trackers.domain.SharedKernel;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;

namespace discipline.daily_trackers.domain.DailyTrackers.BusinessRules.DailyTrackers;

internal sealed class DayCannotBeDefaultRule(DateOnly value) : IBusinessRule
{
    public Exception Exception => new DomainException("DailyTracker.Day.Default",
        "Daily tracker day cannot be default value");

    public bool IsBroken()
        => value == DateOnly.MinValue;
}