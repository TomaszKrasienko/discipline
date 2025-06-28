using discipline.daily_trackers.domain.SharedKernel;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;

namespace discipline.daily_trackers.domain.DailyTrackers.BusinessRules.Stages;

internal sealed class TitleCannotBeLongerThan30(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("DailyTracker.Stage.Title.TooLong",
        $"Title: {value} has invalid length");

    public bool IsBroken()
        => value.Length > 30;
}