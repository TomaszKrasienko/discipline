using discipline.daily_trackers.domain.SharedKernel;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;

namespace discipline.daily_trackers.domain.DailyTrackers.BusinessRules.Stages;

internal sealed class IndexCannotBeLessThan1Rule(int value) : IBusinessRule
{
    public Exception Exception => new DomainException("DailyTracker.Stage.Index.LessThanOne",
        "Stage index cannot be less than 1.");

    public bool IsBroken()
        => value < 1;
}